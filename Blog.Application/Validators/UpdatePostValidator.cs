using Blog.Application.Commands.PostCommands.UpdatePost;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Title).MaximumLength(150).WithMessage("Title must not exceed 150 characters");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
        }
    }
}
