using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommandHandler : IRequestHandler<AddBooksToCardCommand, Result>
    {
        public Task<Result> Handle(AddBooksToCardCommand request, CancellationToken cancellationToken)
        {
            
        }
    }
}
