using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.User.Commands
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, Result>
    {
        private readonly IUserService _userService;

        public UserRegistrationCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RegisterAsync(request);
        }
    }
}
