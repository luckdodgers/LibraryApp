using LibraryApp.Domain;
using LibraryApp.Infrastructure.Identity.Models;

namespace LibraryApp.Infrastructure.Identity
{
    public class DefaultUser : AppUser
    {
        public DefaultUser()
        {
            UserName = "DefaultUser";
            FirstName = "Default";
            LastName = "User";
        }

        public const string default_password = "Qw3rty?!";
        public const Roles default_role = Roles.Reader;
    }
}
