using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Queries.GetCardBooks
{
    public class GetCardBookValidator : AbstractValidator<GetCardBooksQuery>
    {
        public GetCardBookValidator()
        {
            RuleFor(q => q.CardId).NotEmpty();
            RuleFor(q => q.UserName).NotEmpty();
        }
    }
}
