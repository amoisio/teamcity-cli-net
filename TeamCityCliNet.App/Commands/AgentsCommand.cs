using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("agents")]
    public class AgentsCommand : TeamCityCommand
    {
        public AgentsCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "Name" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var agent = await teamCity.BuildAgents.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, agent);
            }
            else
            {
                var agents = await teamCity.BuildAgents.All().ToArrayAsync();
                TablePrinter.Print(console, agents, Fields, Count);
            }
        }
    }

    [Command("agents enable")]
    public class AgentsEnableCommand : ICommand
    {
        private readonly TeamCity _teamCity;
        public AgentsEnableCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandParameter(0, Description = "")]
        public Id Id { get; set; }
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var item = await _teamCity.BuildAgents.ById(Id).ConfigureAwait(false);
            if (!item.Enabled)
            {
                await item.Enable().ConfigureAwait(false);
                console.Output.WriteLine($"Agent {Id} enabled.");
            } 
            else 
            {
                console.Output.WriteLine($"Agent {Id} is already enabled.");
            }
        }
    }

    [Command("agents disable")]
    public class AgentsDisableCommand : ICommand
    {
        private readonly TeamCity _teamCity;
        public AgentsDisableCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandParameter(0, Description = "")]
        public Id Id { get; set; }
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var item = await _teamCity.BuildAgents.ById(Id).ConfigureAwait(false);
            if (item.Enabled)
            {
                await item.Disable().ConfigureAwait(false);
                console.Output.WriteLine($"Agent {Id} disabled.");
            }
            else
            {
                console.Output.WriteLine($"Agent {Id} is already disabled.");
            }
        }
    }

    [Command("agents fields")]
    public class AgentsFieldsCommand : FieldsCommand<IBuildAgent> { }
}