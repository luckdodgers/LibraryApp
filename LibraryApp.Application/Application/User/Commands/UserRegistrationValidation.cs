using FluentValidation;

namespace LibraryApp.Application.User.Commands
{
    public class UserRegistrationValidation : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationValidation()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }
}