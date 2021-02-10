using AutoMapper;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Application.Common.Models;
using LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Queries.GetCardBooks
{
    public class GetCardBooksQueryHandler : IRequestHandler<GetCardBooksQuery, BaseResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCardBooksQueryHandler> _logger;

        public GetCardBooksQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetCardBooksQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResult> Handle(GetCardBooksQuery request, CancellationToken cancellationToken)
        {
            var requestedData = new List<CardBookDto>(0);

            try
            {
                var requestedCard = await _context.Cards.FirstAsync(c => c.UserName == request.UserName);
                await _context.Entry(requestedCard).Collection(c => c.Books).LoadAsync();

                requestedData = _mapper.Map<IReadOnlyCollection<Book>, List<CardBookDto>>(requestedCard.Books);
            }

            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return QueryResult<List<CardBookDto>>.InternalError();
            }

            return QueryResult<List<CardBookDto>>.Success(requestedData);
        }
    }
}
