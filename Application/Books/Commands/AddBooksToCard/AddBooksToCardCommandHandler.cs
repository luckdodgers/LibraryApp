using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommandHandler : IRequestHandler<AddBooksToCardCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddBooksToCardCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddBooksToCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToAdd = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId);

                if (bookToAdd == null)
                    return Result.Fail(RequestError.NotFound, $"Requested book Id={request.BookId} not found");

                var card = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                card.TryAddBook(bookToAdd);

                await _context.SaveChangesAsync();
            }

            catch
            {
                return Result.InternalError();
            }

            return Result.Success();
        }
    }
}