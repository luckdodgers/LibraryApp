using FluentValidation;
using LibraryApp.Application.Application.Common.Models;
using LibraryApp.Application.Common.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : IRequestResult
    {
        private readonly IEnumerable<IValidator> _validators;

        public ValidationBehaviour(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationFailures = _validators
                .Where(validator => validator.CanValidateInstancesOfType(typeof(TRequest)))
                .Select(validator => validator.Validate(new ValidationContext<TRequest>(request)))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .ToList();

            if (validationFailures.Any())
            {
                var error = string.Join("\r\n", validationFailures);
                return (TResponse)(RequestResult.Fail(Enums.RequestError.ValidationError, error) as IRequestResult);
            }

            return await next();
        }
    }
}
