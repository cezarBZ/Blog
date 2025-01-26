using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.LikeCommands.Dislike
{
    public class DislikeCommand : IRequest<Response<Unit>>
    {
        public int Id { get; set; }
    }
}
