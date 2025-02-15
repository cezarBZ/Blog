using Blog.Application.Commands.CommentCommands.UpdateComment;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class UpdateCommentValidator: AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
            RuleFor(x => x.Content).MaximumLength(500).WithMessage("Content must not exceed 500 characters");
        }
    }
}
