using LibraryApp.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace LibraryApp.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected readonly IErrorToStatusCodeConverter _errorToStatusCode;

        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ApiController(IErrorToStatusCodeConverter errorToStatusCode)
        {
            _errorToStatusCode = errorToStatusCode;
        }
    }
}
