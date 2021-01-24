using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("run")]
    public class RunCommand : ICommand
    {
        private readonly TeamCity _teamCity;

        public RunCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandParameter(0, Description = "Build configuration.")]
        public string Configuration { get; set; }

        [CommandOption("number", 'n', Description = "")]
        public string Number { get; set; }

        [CommandOption("personal", Description = "")]
        public bool Personal { get; set; }

        [CommandOption("branch", 'b', Description = "")]
        public string Branch { get; set; }

        [CommandOption("comment", 'c', Description = "")]
        public string Comment { get; set; }

        [CommandOption("rebuidAll", Description = "")]
        public bool RebuildAllDependencies { get; set; }

        [CommandOption("clean", Description = "")]
        public bool? CleanSources { get; set; }

        [CommandOption("top", Description = "")]
        public bool QueueAtTop { get; set; }

        [CommandOption("parameters", 'p', Description = "")]
        public string[] Parameters { get; set; }

        [CommandOption("wait", 'w', Description="")]
        public bool Wait { get; set; }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var parameters = ParseParameters(Parameters);
            var config = await _teamCity.BuildTypes.ById(new Id(Configuration)).ConfigureAwait(false);
            var build = await config.RunBuild(
                parameters,
                QueueAtTop,
                CleanSources,
                RebuildAllDependencies,
                Comment,
                Branch,
                Personal).ConfigureAwait(false);

            ItemPrinter.Print(console, build);

            if (Wait)
            {
                var id = build.Id;
                while(build.State != BuildState.FINISHED)
                {
                    int delay = 1000;
                    await Task.Delay(delay);
                    build = await _teamCity.Builds.ById(id).ConfigureAwait(false);
                    console.Output.WriteLine(build.StatusText);
                }
                console.Output.WriteLine("Build done!");
            }
        }

        private IDictionary<string, string> ParseParameters(string[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            var parameters = new Dictionary<string, string>(values.Length);
            foreach(var parameter in values)
            {
                var components = parameter.Split('=');
                if (components.Length != 2) 
                    continue;
                else
                    parameters.Add(components[0], components[1]);

            }
            return parameters;
        }
    }
}