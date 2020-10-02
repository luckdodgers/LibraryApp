using AutoMapper;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Queries.GetBooksByAuthor
{
    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, List<LibraryBookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBooksByAuthorQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LibraryBookDto>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var books = _context.Authors
                .First(r => r.Name == request.AuthorName)
                .BookAuthors
                .Select(ba => ba.Book);

            if (request.CurrentlyAvailableOnly)
                books = books.Where(b => b.CardId == null);

            return _mapper.Map<IEnumerable<Book>, List<LibraryBookDto>>(books);
        }
    }
}
