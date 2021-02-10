using LibraryApp.Application.Common.Models;
using MediatR;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommand : IRequest<BaseResult>
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
