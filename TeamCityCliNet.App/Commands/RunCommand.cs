using System;
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

        [CommandOption("personal", 'p', Description = "")]
        public bool? Personal { get; set; }

        [CommandOption("branch", 'b', Description = "")]
        public string Branch { get; set; }

        [CommandOption("comment", 'c', Description = "")]
        public string Comment { get; set; }

        [CommandOption("rebuidAll", Description = "")]
        public bool? RebuildAllDependencies { get; set; }

        [CommandOption("clean", Description = "")]
        public bool? CleanSources { get; set; }

        [CommandOption("top", Description = "")]
        public bool? QueueAtTop { get; set; }

        [CommandOption("parameters", 'p', Description = "")]
        public string[] Parameters { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(Parameters);
            // var config = await _teamCity.BuildTypes.ById(new Id(Configuration)).ConfigureAwait(false);
            // config.RunBuild(
            // ItemPrinter.Print(console, build);
        }
    }
}