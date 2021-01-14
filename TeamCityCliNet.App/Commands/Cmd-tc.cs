using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;

namespace TeamCityCliNet.Commands
{
    [Command]
    public class TeamCityCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(nameof(TeamCityCommand));
            return default;
        }
    }
}