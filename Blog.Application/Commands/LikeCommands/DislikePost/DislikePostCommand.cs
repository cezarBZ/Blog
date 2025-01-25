using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.LikeCommands.DislikePost
{
    public class DislikePostCommand : IRequest<Response<Unit>>
    {
        public int Id { get; set; }
    }
}
