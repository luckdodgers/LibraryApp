using LibraryApp.Application.Books.Commands.AddBookToLibrary;
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

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommandHandler : IRequestHandler<AddBooksToCardCommand, CommandResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<AddBooksToCardCommandHandler> _logger;

        public AddBooksToCardCommandHandler(IApplicationDbContext context, ILogger<AddBooksToCardCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CommandResult> Handle(AddBooksToCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToAdd = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId);

                if (bookToAdd == null)
                    return CommandResult.Fail(RequestError.NotFound, $"Requested book Id={request.BookId} not found");

                var card = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                card.TryAddBook(bookToAdd);

                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return CommandResult.InternalError();
            }

            return CommandResult.Success();
        }
    }
}