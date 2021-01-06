using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Behaviours
{
    public class QueryValidationBehaviour<TRequest, QueryResult> : IPipelineBehavior<TRequest, QueryResult> where TRequest : IRequest<QueryResult>
    {
        public Task<QueryResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<QueryResult> next)
        {
            
        }
    }
}
