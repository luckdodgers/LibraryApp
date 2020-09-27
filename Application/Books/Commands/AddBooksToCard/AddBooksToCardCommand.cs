using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardCommand : IRequest<Result>
    {
        public List<int> BookIdList { get; set; }
    }
}
