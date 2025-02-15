using Blog.Application.Commands.CommentCommands.CreateComment;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
            RuleFor(x => x.Content).MaximumLength(500).WithMessage("Content must not exceed 500 characters");
            RuleFor(x => x.postId).NotEmpty().WithMessage("PostId is required");
        }
    }
}
