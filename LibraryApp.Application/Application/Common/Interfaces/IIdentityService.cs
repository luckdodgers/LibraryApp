using LibraryApp.Application.Common.Models;
using LibraryApp.Application.User.Commands;
using LibraryApp.Infrastructure.Identity.Models.Authentication;
using LibraryApp.Infrastructure.Identity.Models.ChangeRole;
using LibraryApp.Infrastructure.Persistance;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<RequestResult> RegisterAsync(UserRegistrationCommand data);

        Task<AuthentificationResponse> GetTokenAsync(TokenRequest request);

        Task<RequestResult> ChangeRoleAsync(ChangeRoleRequest request, RoleActions action);
    }
}
