using LibraryApp.Application.Books.Queries.Common;
using LibraryApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Books.Queries.GetCardBooks
{
    public class GetCardBooksQueryHandler : IRequestHandler<GetCardBooksQuery, List<CardBookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCardBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CardBookDto>> Handle(GetCardBooksQuery request, CancellationToken cancellationToken)
        {
            var requestedCard = await _context.Cards.FirstOrDefaultAsync(c => c.UserName == request.UserName);
            await _context.Entry(requestedCard).Collection(c => c.Books).LoadAsync();

            return _mapper.Map<IReadOnlyCollection<Book>, List<CardBookDto>>(requestedCard.Books);
        }
    }
}
