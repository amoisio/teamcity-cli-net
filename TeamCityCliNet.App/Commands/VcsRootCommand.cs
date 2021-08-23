using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("vcsRoot")]
    public class VcsRootCommand : TeamCityItemCommand<IVcsRoot>
    {
        public VcsRootCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IVcsRoot> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.VcsRoots.ById(Id).ConfigureAwait(false);
    }

    [Command("vcsRoot list")]
    public class VcsRootListCommand : TeamCityListCommand<IVcsRoot>
    {
        public VcsRootListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "Name", "DefaultBranch" };
        
        protected override async Task<IEnumerable<IVcsRoot>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.VcsRoots.All().ToArrayAsync().ConfigureAwait(false);
    }

    [Command("vcsRoot delete")]
    public class VcsRootDeleteCommand : TeamCityItemCommand<IVcsRoot>
    {
        public VcsRootDeleteCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IVcsRoot> Execute(IConsole console, TeamCity teamCity)
        {
            var item = await teamCity.VcsRoots.ById(Id).ConfigureAwait(false);
            await item.Delete().ConfigureAwait(false);
            return item;
        }
    }

    [Command("vcsRoot fields")]
    public class VcsRootFieldsCommand : FieldsCommand<IVcsRoot> { }
}