using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("vcsRoots")]
    public class VcsRootsCommand : TeamCityCommand
    {
        public VcsRootsCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Name", "DefaultBranch" };

        protected override async ValueTask Execute(TeamCity teamCity, Printer printer)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var root = await teamCity.VcsRoots.ById(Id).ConfigureAwait(false);
                printer.PrintAsItem(root, Fields);
            }
            else
            {
                var roots = await teamCity.VcsRoots.All().ToListAsync();
                printer.PrintAsList(roots, Fields);
            }
        }
    }

    [Command("vcsRoots fields")]
    public class VcsRootFieldsCommand : FieldsCommand<IVcsRoot> { }
}