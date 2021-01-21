using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("vcsRoots")]
    public class VcsRootsCommand : TeamCityCommand
    {
        public VcsRootsCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Name", "DefaultBranch" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var root = await teamCity.VcsRoots.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, root);
            }
            else
            {
                var roots = await teamCity.VcsRoots.All().ToArrayAsync();
                TablePrinter.Print(console, roots, Fields, Count);
            }
        }
    }

    [Command("vcsRoots fields")]
    public class VcsRootFieldsCommand : FieldsCommand<IVcsRoot> { }
}