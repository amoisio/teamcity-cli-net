using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("user")]
    public class UserCommand : TeamCityItemCommand<IUser>
    {
        public UserCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IUser> Execute(IConsole console, TeamCity teamCity)
        {
            try
            {
                return await teamCity.Users.ById(Id).ConfigureAwait(false);
            }
            catch (Refit.ApiException)
            {
                return await teamCity.Users.ByUsername(IdString).ConfigureAwait(false);
            }
        }
    }

    [Command("user list")]
    public class UserListCommand : TeamCityListCommand<IUser>
    {
        public UserListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] { "Id", "Username", "Name", "Email" };

        protected override async Task<IEnumerable<IUser>> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.Users.All().ToArrayAsync();
    }

    [Command("user fields")]
    public class UserFieldsCommand : FieldsCommand<IUser> { }
}