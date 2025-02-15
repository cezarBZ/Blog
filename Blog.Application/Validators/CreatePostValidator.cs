using Blog.Application.Commands.PostCommands.CreatePost;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class CreatePostValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Title).MaximumLength(150).WithMessage("Title must not exceed 150 characters");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
        }
    }
}
