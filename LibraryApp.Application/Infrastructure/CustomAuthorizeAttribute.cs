using LibraryApp.Domain;
using Microsoft.AspNetCore.Authorization;

namespace LibraryApp.Infrastructure
{
    public class EnumAuthorizeAttribute : AuthorizeAttribute
    {
        private Roles roleEnum;
        public Roles RoleEnum
        {
            get { return roleEnum; }
            set { roleEnum = value; base.Roles = value.ToString(); }
        }
    }
}
