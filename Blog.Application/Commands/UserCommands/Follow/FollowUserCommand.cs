using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.UserCommands.Follow
{
    public class FollowUserCommand : IRequest<Response<Unit>>
    {
        public FollowUserCommand(int followedId)
        {
            FollowedId = followedId;
        }

        public int FollowedId { get; }
    }
}
