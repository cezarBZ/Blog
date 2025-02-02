using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Commands.UserCommands.Unfollow
{
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, Response<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;

        public UnfollowUserCommandHandler(IUserRepository userRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
        }

        public async Task<Response<Unit>> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            var followerId = _userContextService.GetUserId();

            if (followerId == null)
            {
                return Response<Unit>.Failure("User is not logged in.");
            }

            var follower = await _userRepository.GetByIdWithFollowedAsync(followerId.Value);
            

            if (follower == null)
                return Response<Unit>.Failure("Follower not found.");

            var userToFollow = await _userRepository.GetByIdAsync(request.FollowedId);
            if (userToFollow == null)
                throw new KeyNotFoundException("User to unfollow not found.");

            follower.Unfollow(userToFollow);
            userToFollow.DecrementFollowersCount();

            _userRepository.Update(follower);
            _userRepository.Update(userToFollow);
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Successfully unfollowed the user.");
        }
    }
}
