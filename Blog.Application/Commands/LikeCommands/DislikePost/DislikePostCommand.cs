using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.LikeCommands.DislikePost
{
    public class DislikePostCommand : IRequest<Response<Unit>>
    {
        public int PostId { get; set; }
    }
}
