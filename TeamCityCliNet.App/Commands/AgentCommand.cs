using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("agent")]
    public class AgentShowCommand : TeamCityItemCommand<IBuildAgent>
    {
        public AgentShowCommand(TeamCity teamCity) : base(teamCity) { }
        protected override async Task<IBuildAgent> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.BuildAgents.ById(IdString).ConfigureAwait(false);
    }

    [Command("agent enable")]
    public class AgentEnableCommand : TeamCityItemCommand<IBuildAgent>
    {
        public AgentEnableCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IBuildAgent> Execute(IConsole console, TeamCity teamCity)
        {
            var item = await teamCity.BuildAgents.ById(IdString).ConfigureAwait(false);
            if (!item.Enabled)
                await item.Enable().ConfigureAwait(false);
            return await teamCity.BuildAgents.ById(IdString).ConfigureAwait(false);
        }
    }

    [Command("agent disable")]
    public class AgentDisableCommand : TeamCityItemCommand<IBuildAgent>
    {
        public AgentDisableCommand(TeamCity teamCity) : base(teamCity) { } 

        protected override async Task<IBuildAgent> Execute(IConsole console, TeamCity teamCity)
        {
            var item = await teamCity.BuildAgents.ById(Id).ConfigureAwait(false);
            if (item.Enabled)
                await item.Disable().ConfigureAwait(false);
            return await teamCity.BuildAgents.ById(Id).ConfigureAwait(false);
        }
    }

    [Command("agent fields")]
    public class AgentFieldsCommand : FieldsCommand<IBuildAgent> { }

    [Command("agent list")]
    public class AgentListCommand : TeamCityListCommand<IBuildAgent>
    {
        public AgentListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "Name" };

        protected override async Task<IEnumerable<IBuildAgent>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.BuildAgents.All().ToArrayAsync();
    }

}