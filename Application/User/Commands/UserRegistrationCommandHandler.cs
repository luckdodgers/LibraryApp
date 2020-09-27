using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain.Entities;
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
        private readonly IApplicationDbContext _context;

        public UserRegistrationCommandHandler(IUserService userService, IApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<Result> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.RegisterAsync(request);

            if (result.Succeeded)
            {              
                await _context.Cards.AddAsync(new Card(request.UserName));
                await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
