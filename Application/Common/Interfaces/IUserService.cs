using LibraryApp.Application.Common.Models;
using LibraryApp.Application.User.Commands;
using LibraryApp.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterAsync(UserRegistrationCommand data);

        Task<AuthentificationResponse> GetTokenAsync(TokenRequest request);
    }
}
