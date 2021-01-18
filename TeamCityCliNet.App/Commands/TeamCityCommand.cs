using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    public abstract class TeamCityCommand : ICommand
    {
        private readonly TeamCity _teamCity;
        protected TeamCityCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandOption("id", Description = "Id of the entity to find.")]
        public string Id { get; set; }

        [CommandOption("fields", 'f', Description = "Fields to display.")]
        public string[] Fields { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var printer = new Printer(console);
            await Execute(_teamCity, printer).ConfigureAwait(false);
        }

        protected abstract ValueTask Execute(TeamCity teamCity, Printer printer);
    }
}