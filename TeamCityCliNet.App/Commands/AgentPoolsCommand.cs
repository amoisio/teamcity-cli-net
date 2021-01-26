using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("agentPool")]
    public class AgentPoolCommand : TeamCityItemCommand<IBuildAgentPool>
    {
        public AgentPoolCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IBuildAgentPool> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.BuildAgentPools.ById(Id).ConfigureAwait(false);
    }

    [Command("agentPool list")]
    public class AgentPoolListCommand : TeamCityListCommand<IBuildAgentPool>
    {
        public AgentPoolListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "Name" };

        protected override async Task<IEnumerable<IBuildAgentPool>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.BuildAgentPools.All().ToArrayAsync().ConfigureAwait(false);
    }

    [Command("agentPool fields")]
    public class AgentPoolFieldsCommand : FieldsCommand<IBuildAgentPool> { }
}