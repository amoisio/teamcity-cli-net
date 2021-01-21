using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("agentPools")]
    public class AgentPoolsCommand : TeamCityCommand
    {
        public AgentPoolsCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Name" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var pool = await teamCity.BuildAgentPools.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, pool);
            }
            else
            {
                var pools = await teamCity.BuildAgentPools.All().ToArrayAsync();
                TablePrinter.Print(console, pools, Fields, Count);
            }
        }
    }

    [Command("agentPools fields")]
    public class AgentPoolFieldsCommand : FieldsCommand<IBuildAgentPool> { }
}