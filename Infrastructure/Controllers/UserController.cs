using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(UserRegistrationCommand data)
        {
            var result = await Mediator.Send(data);

            return result.Succeeded ? NoContent() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> GetToken(TokenRequest request)
        {
            var result = await _userService.GetTokenAsync(request);

            return Ok(result);
        }
    }
}