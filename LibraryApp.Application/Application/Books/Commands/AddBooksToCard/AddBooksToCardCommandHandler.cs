﻿using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommandHandler : IRequestHandler<AddBooksToCardCommand, BaseResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<AddBooksToCardCommandHandler> _logger;

        public AddBooksToCardCommandHandler(IApplicationDbContext context, ILogger<AddBooksToCardCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BaseResult> Handle(AddBooksToCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToAdd = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId);

                if (bookToAdd == null)
                    return RequestResult.Fail(RequestError.NotFound, $"Requested book Id={request.BookId} not found");

                var card = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                var isSuccess = card.TryAddBook(bookToAdd);

                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RequestResult.InternalError();
            }

            return RequestResult.Success();
        }
    }
}