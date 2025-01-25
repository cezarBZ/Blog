using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.CommentCommands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<Response<Unit>>
    {
        public int CommentId { get; set; }
    }
}
