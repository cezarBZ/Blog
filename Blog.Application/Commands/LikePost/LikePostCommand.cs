using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.LikePost
{
    public class LikePostCommand : IRequest<Response<Unit>>
    {
        public int postId { get; set; }

    }
}
