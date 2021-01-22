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

        [CommandOption("id", Description = "Id of project whose builds to list.")]
        public override string Id { get; set; }

        public override string[] DefaultFields => new string[] { "Id", "BuildTypeId", "Name" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            Id? projectId = !String.IsNullOrEmpty(Id) ? new Id(Id) : null;
            var items = await teamCity.BuildQueue.All(projectId).ToArrayAsync();
            TablePrinter.Print(console, items, Fields, Count);
        }        
    }

    [Command("queue fields")]
    public class BuildQueueFieldsCommand : FieldsCommand<IBuild> { }
}