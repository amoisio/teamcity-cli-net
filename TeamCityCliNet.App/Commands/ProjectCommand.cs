using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("project")]
    public class ProjectCommand : TeamCityItemCommand<IProject>
    {
        public ProjectCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IProject> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.Projects.ById(Id).ConfigureAwait(false);
    }

    [Command("project list")]
    public class ProjectListCommand : TeamCityListCommand<IProject>
    {
        public ProjectListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "Name", "Description" };

        protected override async Task<IEnumerable<IProject>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.Projects.All().ToArrayAsync();
    }

    [Command("project delete")]
    public class ProjectDeleteCommand : TeamCityItemCommand<IProject>
    {
        public ProjectDeleteCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IProject> Execute(IConsole console, TeamCity teamCity)
        {
            var item = await teamCity.Projects.ById(Id).ConfigureAwait(false);
            await item.Delete().ConfigureAwait(false);
            return item;
        }
    }

    [Command("project fields")]
    public class ProjectFieldsCommand : FieldsCommand<IProject> { }
}