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

        [CommandParameter(0, Description = "Username of the user to find.")]
        public string Type { get; set; }

        [CommandOption("number", 'n', Description = "")]
        public string Number { get; set; }


        public async ValueTask ExecuteAsync(IConsole console)
        {
            var build = await _teamCity.Build(new Id(Type), Number).ConfigureAwait(false);
            ItemPrinter.Print(console, build);
        }
    }
}