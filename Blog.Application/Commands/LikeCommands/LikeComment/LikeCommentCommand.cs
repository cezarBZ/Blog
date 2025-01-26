using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.LikeCommands.LikeComment
{
    public class LikeCommentCommand : IRequest<Response<Unit>>
    {
        public int commentId { get; set; }
    }
}
