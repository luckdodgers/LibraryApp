using FluentValidation;

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
