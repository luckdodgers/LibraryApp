using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardValidator : AbstractValidator<AddBooksToCardCommand>
    {
        public AddBooksToCardValidator()
        {
            RuleFor(c => c.BookId).NotEmpty();
            RuleFor(c => c.UserName).NotEmpty();
        }
    }
}
