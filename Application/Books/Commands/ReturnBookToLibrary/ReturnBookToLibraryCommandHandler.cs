using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.ReturnBookToLibrary
{
    public class ReturnBookToLibraryCommandHandler : IRequestHandler<ReturnBookToLibraryCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ReturnBookToLibraryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ReturnBookToLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedCard = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                var bookToRemove = requestedCard.Books.FirstOrDefault(b => b.Id == request.BookId);
                requestedCard.TryRemoveBook(bookToRemove);

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