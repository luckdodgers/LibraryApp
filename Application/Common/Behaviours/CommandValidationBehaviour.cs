using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Behaviours
{
    public class CommandValidationBehaviour<TRequest, CmdResult> : IPipelineBehavior<TRequest, CmdResult> where TRequest : IRequest<CmdResult> where CmdResult : 
    {
        public async Task<CmdResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<CmdResult> next)
        {
            
        }
    }
}
