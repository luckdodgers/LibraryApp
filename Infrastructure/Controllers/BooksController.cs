using LibraryApp.Application.Books.Commands.AddBooksToCard;
using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.GetBooksByAuthor;
using LibraryApp.Application.Books.Queries.GetCardBooks;
using LibraryApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var username = GetUsername();
            return await Mediator.Send(new GetCardBooksQuery(cardId, username));
        }

        [AllowAnonymous]
        public async Task<ActionResult<List<LibraryBookDto>>> GetBooksByAuthor(GetBooksByAuthorQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> AddToCard(int bookId)
        {
            var username = GetUsername();
            var result = await Mediator.Send(new AddBooksToCardCommand(bookId, username));
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [HttpPost]
        [Route("[action]/cardId={cardId}")]
        public async Task<ActionResult> ReturnToLibrary(int cardId)
        {
            var username = GetUsername();
            var result = await Mediator.Send(new ReturnBookToLibraryCommand(cardId, username));
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }

        [HttpPost("[action]")]
        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> AddToLibrary(AddBookToLibraryCommand request)
        {
            var result = await Mediator.Send(request);
            return result.Succeeded ? Ok() : (ActionResult)BadRequest(result.ErrorsToString());
        }
    }
}
