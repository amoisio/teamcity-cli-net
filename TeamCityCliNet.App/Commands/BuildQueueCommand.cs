using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("queue")]
    public class BuildQueueCommand : TeamCityCommand
    {
        public BuildQueueCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "BuildTypeId", "Name" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            var items = await teamCity.BuildQueue.All().ToArrayAsync();
            if (!String.IsNullOrEmpty(Id))
            {
                items = items.Where(i => Id.Equals(i.Id.StringId, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            }
            TablePrinter.Print(console, items, Fields, Count);
        }        
    }

    [Command("queue fields")]
    public class BuildQueueFieldsCommand : FieldsCommand<IBuild> { }
}