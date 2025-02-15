using Blog.Application.Commands.UserCommands.CreateUser;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Username).MaximumLength(50).WithMessage("Username must not exceed 50 characters");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            RuleFor(x => x.Password).MaximumLength(50).WithMessage("Password must not exceed 50 characters");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not valid");

        }

    }
}
