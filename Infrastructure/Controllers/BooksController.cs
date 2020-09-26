using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.Common;
using LibraryApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    [Authorize]
    public class BooksController : ApiController
    {
        [HttpGet("cardId={cardId}")]
        public async Task<ActionResult<List<CardBookDto>>> GetCardBooks(int cardId)
        {
            return await Mediator.Send(new GetCardBooksQuery(cardId));
        }

        [HttpPost("[action]")]
        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> AddToLibrary(AddBookCommand request)
        {
            var result = await Mediator.Send(request);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }
    }
}
