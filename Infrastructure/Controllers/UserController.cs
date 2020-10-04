using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain;
using LibraryApp.Infrastructure.Identity.Models.Authentication;
using LibraryApp.Infrastructure.Identity.Models.ChangeRole;
using LibraryApp.Infrastructure.Interfaces;
using LibraryApp.Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IErrorToStatusCodeConverter errorToStatusCode) : base(errorToStatusCode)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserRegistrationCommand data)
        {
            var result = await Mediator.Send(data);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken(TokenRequest request)
        {
            var result = await _userService.GetTokenAsync(request);
            return result.IsAuthorized ? Ok(result) : (ActionResult)BadRequest(result.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> AddRole(ChangeRoleRequest request)
        {
            var result = await _userService.ChangeRoleAsync(request, RoleActions.Add);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [HttpPost]
        [Route("[action]")]
        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> RemoveRole(ChangeRoleRequest request)
        {
            var result = await _userService.ChangeRoleAsync(request, RoleActions.Remove);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }
    }
}