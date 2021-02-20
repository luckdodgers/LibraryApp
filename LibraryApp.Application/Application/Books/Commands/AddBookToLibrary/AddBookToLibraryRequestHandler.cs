using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBookToLibrary
{
    public class AddBookToLibraryRequestHandler : IRequestHandler<AddBookToLibraryCommand, BaseResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<AddBookToLibraryRequestHandler> _logger;

        public AddBookToLibraryRequestHandler(IApplicationDbContext context, ILogger<AddBookToLibraryRequestHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BaseResult> Handle(AddBookToLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.Books.Where(b => b.Title == request.Title && b.Authors.Any(ba => request.Authors.Any(ra => ra == ba.Name))).Any())
                    return RequestResult.Fail(RequestError.AlreadyExists, $"Book with title {request.Title} and authors {string.Join(", ", request.Authors)} already exist");

                // Caching authors of new book
                var authors = new List<Author>(request.Authors.Count);

                // Get authors or create if not exist
                foreach (var authorName in request.Authors)
                {
                    var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == authorName);

                    if (author == null)
                    {
                        author = new Author(authorName);
                        await _context.Authors.AddAsync(author);
                        await _context.SaveChangesAsync();
                    }

                    authors.Add(author);
                }

                var book = new Book(request.Title);
                book.SetAuthors(authors);
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