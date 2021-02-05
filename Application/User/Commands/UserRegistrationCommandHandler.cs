using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.User.Commands
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, RequestResult>
    {
        private readonly IIdentityService _userService;
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UserRegistrationCommandHandler> _logger;

        public UserRegistrationCommandHandler(IIdentityService userService, IApplicationDbContext context, ILogger<UserRegistrationCommandHandler> logger)
        {
            _userService = userService;
            _context = context;
            _logger = logger;
        }

        public async Task<RequestResult> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            RequestResult result;

            try
            {
                result = await _userService.RegisterAsync(request);

                if (result.Succeeded)
                {
                    await _context.Cards.AddAsync(new Card(request.UserName));
                    await _context.SaveChangesAsync();
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RequestResult.InternalError();
            }

            return result;
        }
    }
}
