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
                var card = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                var bookToAdd = await _context.Books.FirstAsync(b => b.Id == request.BookId);
                card.TryAddBook(bookToAdd);

                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                return Result.Fail("Internal error");
            }

            return Result.Success();
        }
    }
}