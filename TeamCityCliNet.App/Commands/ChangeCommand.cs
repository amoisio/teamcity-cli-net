using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("change")]
    public class ChangeCommand : TeamCityItemCommand<IChange>
    {
        public ChangeCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IChange> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.Changes.ById(Id).ConfigureAwait(false);
    }

    [Command("change type")]
    public class ChangeTypeCommand : ICommand
    {
        private readonly TeamCity _teamCity;

        public ChangeTypeCommand(TeamCity teamCity) 
        { 
            _teamCity = teamCity;
        }

        [CommandParameter(0, Name = "type", Description = "Build type id.")]
        public string BuildTypeId { get; set; }

        [CommandParameter(1, Name = "version", Description = "Version.")]
        public string Version { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var item = await _teamCity.Changes.ByBuildTypeId(new Id(BuildTypeId), Version).ConfigureAwait(false);
            ItemPrinter.Print(console, item);
        }
    }

    [Command("change list")]
    public class ChangeListCommand : TeamCityListCommand<IChange>
    {
        public ChangeListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Version", "Username", "Date" };

        protected override async Task<IEnumerable<IChange>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.Changes.All().ToArrayAsync().ConfigureAwait(false);
    }

    [Command("change fields")]
    public class ChangeFieldsCommand : FieldsCommand<IChange> { }
}