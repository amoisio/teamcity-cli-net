using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("users")]
    public class UsersCommand: ICommand
    {
        private readonly TeamCity _teamCity;

        public UsersCommand(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        [CommandOption("username",'u', Description = "Username of the user to find.")]
        public string Username { get; set; }

        [CommandOption("id", Description = "Id of the user to find.")]
        public string Id { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (!String.IsNullOrEmpty(Username))
            {
                var user = await _teamCity.Users.ByUsername(Username).ConfigureAwait(false);
                PrintUser(user, console);
            } 
            else if(!String.IsNullOrEmpty(Id))
            {
                var user = await _teamCity.Users.ById(Id).ConfigureAwait(false);
                PrintUser(user, console);
            } 
            else 
            {
                var users = _teamCity.Users.All();
                await foreach (var user in users)
                    PrintUser(user, console);
            }
        }

        private void PrintUser(IUser user, IConsole console)
        {
            console.Output.WriteLine($"{user.Id} {user.Username} {user.Name} ({user.Email})");
        }
    }
}