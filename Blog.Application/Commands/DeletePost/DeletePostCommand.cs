using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<Response<Unit>>
    {
        public DeletePostCommand(int postId)
        {
            PostId = postId;
        }
        public int PostId { get; set; }
    }
}
