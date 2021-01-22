using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("buildTypes")]
    public class BuildTypesCommand : TeamCityCommand
    {
        public BuildTypesCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Name", "ProjectId", "ProjectName" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var item = await teamCity.BuildTypes.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, item);
            }
            else
            {
                var items = await teamCity.BuildTypes.All().ToArrayAsync();
                TablePrinter.Print(console, items, Fields, Count);
            }
        }
    }

    [Command("buildTypes fields")]
    public class BuildTypeFieldsCommand : FieldsCommand<IBuildType> { }
}