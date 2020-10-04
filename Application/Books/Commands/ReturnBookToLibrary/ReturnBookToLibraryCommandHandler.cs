using LibraryApp.Application.Common.Enums;
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
                var requestedCard = await _context.Cards.Where(c => c.UserName == request.UserName).Include(c => c.Books).FirstAsync();
                var bookToRemove = requestedCard.Books.FirstOrDefault(b => b.Id == request.BookId);

                if (bookToRemove == null)
                    return Result.Fail(RequestError.NotFound, $"Requested book Id={request.BookId} not found in card");

                requestedCard.TryRemoveBook(bookToRemove);

                await _context.SaveChangesAsync();
            }

            catch
            {
                return Result.Fail(RequestError.ApplicationException, "Internal error");
            }

            return Result.Success();
        }
    }
}