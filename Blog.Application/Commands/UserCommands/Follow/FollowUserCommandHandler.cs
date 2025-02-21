using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Commands.UserCommands.Follow
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Response<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;

        public FollowUserCommandHandler(IUserRepository userRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
        }

        public async Task<Response<Unit>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            var followerId = _userContextService.GetUserId();
            var follower = await _userRepository.GetByIdAsync(followerId.Value);

            if (follower == null)
                return Response<Unit>.Failure("Follower not found.");

            var userToFollow = await _userRepository.GetByIdAsync(request.FollowedId);
            if (userToFollow == null)
                return Response<Unit>.Failure("User to follow not found.");

            follower.Follow(userToFollow);

            _userRepository.Update(follower);
            _userRepository.Update(userToFollow);
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Response<Unit>.Success(Unit.Value, "Successfully followed the user.");
        }
    }
}
