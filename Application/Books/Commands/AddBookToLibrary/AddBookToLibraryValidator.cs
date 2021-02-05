using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBookToLibrary
{
    public class AddBookToLibraryValidator : AbstractValidator<AddBookToLibraryCommand>
    {
        public AddBookToLibraryValidator()
        {
            RuleFor(c => c.Authors).NotEmpty();
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
