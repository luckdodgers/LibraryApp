using LibraryApp.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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

        protected string GetUsername()
        {
            return this.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
    }
}
