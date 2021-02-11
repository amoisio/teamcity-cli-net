using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    public abstract class TeamCityItemCommand<TItem> : ICommand
    {
        private readonly TeamCity _teamCity;
        protected TeamCityItemCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandParameter(0, Name="Id")]
        public virtual string IdString { get; set; }
        public Id Id => new(IdString);

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var item = await Execute(console, _teamCity).ConfigureAwait(false);
            ItemPrinter.Print(console, item);
        }

        protected abstract Task<TItem> Execute(IConsole console, TeamCity teamCity);
    }
}