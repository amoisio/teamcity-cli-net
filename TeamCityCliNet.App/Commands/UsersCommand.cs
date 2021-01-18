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

        private string[] EffectiveFields
        {
            get{
                if (Fields != null && Fields.Length > 0)
                    return Fields;
                else
                    return new string[] {"Id", "Username", "Name", "Email" };
            }
        }

        protected override async ValueTask Execute(TeamCity teamCity, Printer printer)
        {
            if (!String.IsNullOrEmpty(Username))
            {
                var user = await teamCity.Users.ByUsername(Username).ConfigureAwait(false);
                printer.PrintAsItem(user, EffectiveFields);
            }
            else if (!String.IsNullOrEmpty(Id))
            {
                var user = await teamCity.Users.ById(Id).ConfigureAwait(false);
                printer.PrintAsItem(user, EffectiveFields);
            }
            else
            {
                var users = await teamCity.Users.All().ToListAsync();
                printer.PrintAsList(users, EffectiveFields);
            }
        }
    }
}