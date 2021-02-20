using FluentValidation;

namespace LibraryApp.Application.Books.Commands.ReturnBookToLibrary
{
    public class ReturnBookToLibraryValidator : AbstractValidator<ReturnBookToLibraryCommand>
    {
        public ReturnBookToLibraryValidator()
        {
            RuleFor(c => c.BookId).GreaterThanOrEqualTo(0);
            RuleFor(c => c.UserName).NotEmpty();
        }
    }
}
