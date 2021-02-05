using FluentValidation;

namespace LibraryApp.Application.Books.Queries.GetBooksByAuthor
{
    public class GetBooksByAuthorValidator : AbstractValidator<GetBooksByAuthorQuery>
    {
        public GetBooksByAuthorValidator()
        {
            RuleFor(c => c.AuthorName).NotEmpty();
        }
    }
}
