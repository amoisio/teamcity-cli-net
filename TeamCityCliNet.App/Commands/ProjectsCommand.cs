using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("projects")]
    public class ProjectsCommand : TeamCityCommand
    {
        public ProjectsCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "Name", "Description" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (Id != null)
            {
                var item = await teamCity.Projects.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, item);
            }
            else
            {
                var items = await teamCity.Projects.All().ToArrayAsync();
                TablePrinter.Print(console, items, Fields, Count);
            }
        }
    }

    [Command("projects delete")]
    public class ProjectDeleteCommand : ICommand
    {
        private readonly TeamCity _teamCity;
        public ProjectDeleteCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandParameter(0, Description = "")]
        public Id Id { get; set; }
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var item = await _teamCity.Projects.ById(Id).ConfigureAwait(false);
            await item.Delete().ConfigureAwait(false);
            console.Output.WriteLine(item.Id);
        }
    }

    [Command("projects fields")]
    public class ProjectFieldsCommand : FieldsCommand<IProject> { }
}