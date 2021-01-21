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

        public override string[] DefaultFields => new string[] {"Id", "Name" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var agent = await teamCity.BuildAgents.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, agent, Fields);
            }
            else
            {
                var agents = await teamCity.BuildAgents.All().ToArrayAsync();
                TablePrinter.Print(console, agents, Fields, Count);
            }
        }
    }

    [Command("agents fields")]
    public class AgentFieldsCommand : FieldsCommand<IBuildAgent> { }
}