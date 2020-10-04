using AutoMapper;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Queries.GetCardBooks
{
    public class GetCardBooksQueryHandler : IRequestHandler<GetCardBooksQuery, (Result, List<CardBookDto>)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCardBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(Result, List<CardBookDto>)> Handle(GetCardBooksQuery request, CancellationToken cancellationToken)
        {
            var requestedData = new List<CardBookDto>(0);

            try
            {
                var requestedCard = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                await _context.Entry(requestedCard).Collection(c => c.Books).LoadAsync();

                requestedData = _mapper.Map<IReadOnlyCollection<Book>, List<CardBookDto>>(requestedCard.Books);
            }

            catch
            {
                return (Result.InternalError(), requestedData);
            }

            return (Result.Success(), requestedData);
        }
    }
}
