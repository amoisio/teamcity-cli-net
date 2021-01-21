using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("users")]
    public class UsersCommand : TeamCityCommand
    {
        public UsersCommand(TeamCity teamCity) : base(teamCity) { }

        [CommandOption("username",'u', Description = "Username of the user to find.")]
        public string Username { get; set; }

        public override string[] DefaultFields => new string[] {"Id", "Username", "Name", "Email" };

        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Username))
            {
                var user = await teamCity.Users.ByUsername(Username).ConfigureAwait(false);
                ItemPrinter.Print(console, user);
            }
            else if (!String.IsNullOrEmpty(Id))
            {
                var user = await teamCity.Users.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, user);
            }
            else
            {
                var users = await teamCity.Users.All().ToArrayAsync();
                TablePrinter.Print(console, users, Fields, Count);
            }
        }
    }

    [Command("users fields")]
    public class UserFieldsCommand : FieldsCommand<IUser> { }
}