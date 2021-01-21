using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("changes")]
    public class ChangesCommand : TeamCityCommand
    {
        public ChangesCommand(TeamCity teamCity) : base(teamCity) { }

        [CommandOption("type", 't', Description = "Build type id.")]
        public string BuildTypeId { get; set; }


        [CommandOption("version", 'v', Description = "Version.")]
        public string Version { get; set; }

        public override string[] DefaultFields => new string[] {"Id", "Version", "Username", "Date" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var change = await teamCity.Changes.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, change);
            }
            else if (!String.IsNullOrEmpty(BuildTypeId) || !String.IsNullOrEmpty(Version))
            {
                var change = await teamCity.Changes.ByBuildTypeId(new Id(BuildTypeId), Version).ConfigureAwait(false);
                ItemPrinter.Print(console, change);
            }
            else
            {
                var changes = await teamCity.Changes.All().ToArrayAsync();
                TablePrinter.Print(console, changes, Fields, Count);
            }
        }
    }

    [Command("changes fields")]
    public class ChangeFieldsCommand : FieldsCommand<IChange> { }
}