using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.ReturnBookToLibrary
{
    public class ReturnBookToLibraryCommandHandler : IRequestHandler<ReturnBookToLibraryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ReturnBookToLibraryCommandHandler> _logger;

        public ReturnBookToLibraryCommandHandler(IApplicationDbContext context, ILogger<ReturnBookToLibraryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
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

            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return Result.Fail(RequestError.ApplicationException, "Internal error");
            }

            return Result.Success();
        }
    }
}