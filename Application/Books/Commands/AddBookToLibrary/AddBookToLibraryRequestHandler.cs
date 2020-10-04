using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBookToLibrary
{
    public class AddBookToLibraryRequestHandler : IRequestHandler<AddBookToLibraryCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddBookToLibraryRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddBookToLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync();

                if (await _context.Books.AnyAsync(b => b.Title == request.Title && b.BookAuthors == request.Authors))
                    return Result.Fail(RequestError.AlreadyExists, $"Book with title {request.Title} and authors {string.Join(", ", request.Authors)} already exist");

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

                // Adding new book
                book = new Book(request.Title);
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                // Caching join entity
                var bookAuthors = new List<BookAuthor>(request.Authors.Count);

                // Adding join entites to Authors
                foreach (var author in authors)
                {
                    var bookAuthor = new BookAuthor(authorId: author.Id, author: author, bookId: book.Id, book: book);
                    bookAuthors.Add(bookAuthor);
                    author.AddBook(bookAuthor);

                    await _context.BookAuthors.AddAsync(bookAuthor);
                }

                // Adding join entites to Book
                book.SetAuthors(bookAuthors);
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