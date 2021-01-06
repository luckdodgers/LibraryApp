using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommand : IRequest<CommandResult>
    {
        public AddBooksToCardCommand(int bookId, string userName)
        {
            BookId = bookId;
            UserName = userName;
        }

        public int BookId { get; }
        public string UserName { get; }
    }
}
