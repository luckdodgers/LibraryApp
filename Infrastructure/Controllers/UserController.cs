using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models;
using LibraryApp.Infrastructure.Identity.Models.AddRole;
using LibraryApp.Infrastructure.Identity.Models.Authentication;
using LibraryApp.Infrastructure.Identity.Models.ChangeRole;
using LibraryApp.Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    [Authorize]
    [Route("[action]")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Register(UserRegistrationCommand data)
        {
            var result = await Mediator.Send(data);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetToken(TokenRequest request)
        {
            var result = await _userService.GetTokenAsync(request);
            return Ok(result);
        }

        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> AddRole(ChangeRoleRequest request)
        {
            var result = await _userService.ChangeRoleAsync(request, RoleActions.Add);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> RemoveRole(ChangeRoleRequest request)
        {
            var result = await _userService.ChangeRoleAsync(request, RoleActions.Remove);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }
    }
}