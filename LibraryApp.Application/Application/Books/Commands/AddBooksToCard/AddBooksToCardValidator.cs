﻿using FluentValidation;

namespace LibraryApp.Application.Books.Commands.AddBooksToCard
{
    public class AddBooksToCardValidator : AbstractValidator<AddBooksToCardCommand>
    {
        public AddBooksToCardValidator()
        {
            RuleFor(c => c.BookId).GreaterThanOrEqualTo(0);
            RuleFor(c => c.UserName).NotEmpty();
        }
    }
}
