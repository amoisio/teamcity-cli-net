using System;
using System.Linq;
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
            var printer = new Printer(console);

            if (!String.IsNullOrEmpty(Username))
            {
                var user = await _teamCity.Users.ByUsername(Username).ConfigureAwait(false);
                printer.PrintAsItem(user, "Id", "Username", "Name", "Email");
            } 
            else if(!String.IsNullOrEmpty(Id))
            {
                var user = await _teamCity.Users.ById(Id).ConfigureAwait(false);
                printer.PrintAsItem(user, "Id", "Username", "Name", "Email");
            } 
            else 
            {
                var users = await _teamCity.Users.All().ToListAsync();
                printer.PrintAsList(users, "Id", "Username");
            }
        }
    }
}