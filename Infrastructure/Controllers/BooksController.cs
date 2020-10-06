﻿using LibraryApp.Application.Books.Commands.AddBooksToCard;
using LibraryApp.Application.Books.Commands.AddBookToLibrary;
using LibraryApp.Application.Books.Commands.ReturnBookToLibrary;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.GetBooksByAuthor;
using LibraryApp.Application.Books.Queries.GetCardBooks;
using LibraryApp.Domain;
using LibraryApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    [Authorize]
    public class BooksController : ApiController
    {
        private readonly ICurrentUserService _currentUser;

        public BooksController(IErrorToStatusCodeConverter errorToStatusCode, ICurrentUserService currentUser) : base(errorToStatusCode)
        {
            _currentUser = currentUser;
        }

        [HttpGet("cardId={cardId}")]
        public async Task<ActionResult<List<CardBookDto>>> GetCardBooks(int cardId)
        {
            var username = _currentUser.UserName;
            //var username = GetUsername();
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
        [Route("[action]/bookid={bookId}")]
        public async Task<ActionResult> AddToCard(int bookId) // Ok
        {
            var username = _currentUser.UserName;
            //var username = GetUsername();
            var result = await Mediator.Send(new AddBooksToCardCommand(bookId, username));

            return result.Succeeded ? NoContent() : StatusCode(_errorToStatusCode.Convert(result.ErrorType));
        }

        [HttpPost]
        [Route("[action]/bookId={bookId}")]
        public async Task<ActionResult> ReturnToLibrary(int bookId) // Ok
        {
            var username = _currentUser.UserName;
            //var username = GetUsername();
            var result = await Mediator.Send(new ReturnBookToLibraryCommand(bookId, username));

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
