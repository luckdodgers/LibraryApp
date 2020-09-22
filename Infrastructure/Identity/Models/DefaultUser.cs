using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public const string default_password = "qwerty";
        public const Roles default_role = Roles.Reader;
    }
}
