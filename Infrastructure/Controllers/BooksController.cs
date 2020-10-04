using LibraryApp.Application.Books.Commands.AddBooksToCard;
using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.GetBooksByAuthor;
using LibraryApp.Application.Books.Queries.GetCardBooks;
using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain;
using LibraryApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    [Authorize]
    public class BooksController : ApiController
    {
        public BooksController(IErrorToStatusCodeConverter errorToStatusCode) : base(errorToStatusCode)
        {
        }

        [HttpGet("cardId={cardId}")]
        public async Task<ActionResult<List<CardBookDto>>> GetCardBooks(int cardId)
        {
            var username = GetUsername();
            var response = await Mediator.Send(new GetCardBooksQuery(cardId, username));

            var result = response.Item1;
            var requestedData = response.Item2;

            if (result.Succeeded)
                return requestedData;

            return StatusCode(_errorToStatusCode.Convert(result.ErrorType));
        }

        [AllowAnonymous]
        public async Task<ActionResult<List<LibraryBookDto>>> GetBooksByAuthor(GetBooksByAuthorQuery request)
        {
            var response = await Mediator.Send(request);

            var result = response.Item1;
            var requestedData = response.Item2;

            if (result.Succeeded)
                return requestedData;

            return StatusCode(_errorToStatusCode.Convert(result.ErrorType));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> AddToCard(int bookId)
        {
            var username = GetUsername();
            var result = await Mediator.Send(new AddBooksToCardCommand(bookId, username));

            return result.Succeeded ? NoContent() : StatusCode(_errorToStatusCode.Convert(result.ErrorType));
        }

        [HttpPost]
        [Route("[action]/cardId={cardId}")]
        public async Task<ActionResult> ReturnToLibrary(int cardId)
        {
            var username = GetUsername();
            var result = await Mediator.Send(new ReturnBookToLibraryCommand(cardId, username));

            return result.Succeeded ? NoContent() : StatusCode(_errorToStatusCode.Convert(result.ErrorType));
        }

        [HttpPost("[action]")]
        [EnumAuthorize(RoleEnum = Roles.Admin)]
        public async Task<ActionResult> AddToLibrary(AddBookToLibraryCommand request)
        {
            var result = await Mediator.Send(request);
            return result.Succeeded ? NoContent() : StatusCode(_errorToStatusCode.Convert(result.ErrorType));
        }
    }
}
