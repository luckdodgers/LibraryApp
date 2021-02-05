using LibraryApp.Application.Common.Models;
using MediatR;

namespace LibraryApp.Application.Books.Commands.ReturnBookToLibrary
{
    public class ReturnBookToLibraryCommand : IRequest<BaseResult>
    {
        public int BookId { get; }
        public string UserName { get; }

        public ReturnBookToLibraryCommand(int bookId, string userName)
        {
            BookId = bookId;
            UserName = userName;
        }
    }
}
