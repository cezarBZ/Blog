using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.LikeCommands.DislikeComment
{
    public class DislikeCommentCommand : IRequest<Response<Unit>>
    {
        public int CommentId { get; set; }
    }
}
