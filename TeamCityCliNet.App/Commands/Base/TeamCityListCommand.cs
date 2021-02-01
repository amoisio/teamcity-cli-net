using CliFx.Attributes;
using CliFx;
using TeamCityRestClientNet.Api;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TeamCityCliNet.Commands
{
    public abstract class TeamCityListCommand<T> : ICommand
    {
        private readonly TeamCity _teamCity;
        protected TeamCityListCommand(TeamCity teamCity) 
        { 
            _teamCity = teamCity;
        }

        private string[] _fields;

        [CommandOption("fields", 'f', Description = "Fields to display.")]
        public string[] Fields
        {
            get => (_fields == null || _fields.Length == 0)
                ? DefaultFields
                : _fields;
            set => _fields = value;
        }
        public abstract string[] DefaultFields { get; }

        [CommandOption("count", 'c', Description = "Number of items to display.")]
        public virtual int Count { get; set; } = 30;

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var items = await Execute(console, _teamCity).ConfigureAwait(false);
            TablePrinter.Print(console, items.ToArray(), Fields, Count);
        }

        protected abstract Task<IEnumerable<T>> Execute(IConsole console, TeamCity teamCity);
    }
}