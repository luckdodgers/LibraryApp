using FluentValidation;

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
