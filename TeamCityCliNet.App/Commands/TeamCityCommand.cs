using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    public abstract class TeamCityCommand : ICommand
    {
        private readonly TeamCity _teamCity;
        protected TeamCityCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandOption("id", Description = "Id of the entity to find.")]
        public virtual string Id { get; set; }

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
        public virtual int? Count { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await Execute(console, _teamCity).ConfigureAwait(false);
        }

        protected abstract ValueTask Execute(IConsole console, TeamCity teamCity);
    }
}