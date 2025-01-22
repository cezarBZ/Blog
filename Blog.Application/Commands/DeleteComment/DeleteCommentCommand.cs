using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<Response<Unit>>
    {
        public int CommentId { get; set; }
    }
}
