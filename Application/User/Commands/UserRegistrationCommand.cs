using LibraryApp.Application.Common.Models;
using MediatR;

namespace LibraryApp.Application.User.Commands
{
    public class UserRegistrationCommand : IRequest<Result>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
