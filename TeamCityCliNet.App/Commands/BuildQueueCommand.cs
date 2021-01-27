using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("queue")]
    public class BuildQueueCommand : TeamCityListCommand<IBuild>
    {
        public BuildQueueCommand(TeamCity teamCity) : base(teamCity) { }

        [CommandOption("projectId", Description = "Id of project whose builds to list.")]
        public string ProjectId { get; set; }
        public override string[] DefaultFields => new string[] { "Id", "BuildTypeId", "Name" };
        protected override async Task<IEnumerable<IBuild>> Execute(IConsole console, TeamCity teamCity)
        {
            Id? projectId = !String.IsNullOrEmpty(ProjectId) ? new Id(ProjectId) : null;
            return await teamCity.BuildQueue.All(projectId).ToArrayAsync().ConfigureAwait(false);
        }        
    }

    [Command("queue fields")]
    public class BuildQueueFieldsCommand : FieldsCommand<IBuild> { }
}