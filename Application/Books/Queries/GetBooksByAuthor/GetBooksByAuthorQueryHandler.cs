using AutoMapper;
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

namespace LibraryApp.Application.Books.Queries.GetBooksByAuthor
{
    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, QueryResult<List<LibraryBookDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBooksByAuthorQueryHandler> _logger;

        public GetBooksByAuthorQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetBooksByAuthorQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<QueryResult<List<LibraryBookDto>>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var requestedData = new List<LibraryBookDto>(0);

            try
            {
                var author = await _context.Authors.FirstOrDefaultAsync(r => r.Name == request.AuthorName);

                if (author == null)
                    return QueryResult<List<LibraryBookDto>>.Fail(RequestError.NotFound, $"Requested author {request.AuthorName} not found");

                await _context.Entry(author).Collection(a => a.BookAuthors).LoadAsync();
                var books = author.BookAuthors.Select(ba => ba.Book);

                if (request.CurrentlyAvailableOnly)
                    books = books.Where(b => b.CardId == null);

                requestedData = _mapper.Map<IEnumerable<Book>, List<LibraryBookDto>>(books);
            }

            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return QueryResult<List<LibraryBookDto>>.InternalError();
            }

            return QueryResult<List<LibraryBookDto>>.Success(requestedData);
        }
    }
}