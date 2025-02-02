using Blog.Application.Responses;
using MediatR;

namespace Blog.Application.Commands.UserCommands.Unfollow
{
    public class UnfollowUserCommand : IRequest<Response<Unit>>
    {
        public UnfollowUserCommand(int followedId)
        {
            FollowedId = followedId;
        }

        public int FollowedId { get; }
    }
}
