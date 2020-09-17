using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Books.Queries.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Controllers
{
    public class BooksController : ApiController
    {
        [HttpGet("cardId={cardId}")]
        public async Task<ActionResult<List<CardBookDto>>> GetCardBooks(int cardId)
        {
            return await Mediator.Send(new GetCardBooksQuery(cardId));
        }
    }
}
