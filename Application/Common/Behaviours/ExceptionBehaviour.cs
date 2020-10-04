using LibraryApp.Application.Common.Models;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Behaviours
{
    public class ExceptionBehaviour<TRequest, TResult> : IRequestExceptionHandler<TRequest, TResult>
    {
        private readonly ILogger<TRequest> _logger;

        public ExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task Handle(TRequest request, Exception exception, RequestExceptionHandlerState<TResult> state, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(exception, "Exception for Request {Name} {@Request}", requestName, request);

            throw exception;
        }
    }
}
