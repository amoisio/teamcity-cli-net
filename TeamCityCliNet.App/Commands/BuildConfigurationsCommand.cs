using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("configuration")]
    public class BuildConfigurationCommand : TeamCityItemCommand<IBuildType>
    {
        public BuildConfigurationCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IBuildType> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.BuildTypes.ById(Id).ConfigureAwait(false);
    }

    [Command("configuration list")]
    public class BuildConfigurationListCommand : TeamCityListCommand<IBuildType>
    {
        public BuildConfigurationListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Name", "ProjectId", "ProjectName" };

        protected override async Task<IEnumerable<IBuildType>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.BuildTypes.All().ToArrayAsync().ConfigureAwait(false);
    }

    [Command("configuration fields")]
    public class BuildConfigurationFieldsCommand : FieldsCommand<IBuildType> { }
}