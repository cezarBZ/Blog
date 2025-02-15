using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, Response<IReadOnlyList<UserResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<IReadOnlyList<UserResponse>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Response<IReadOnlyList<UserResponse>>.NotFound("Usuário não encontrado.");
            }

            var followers = await _userRepository.GetFollowersAsync(request.userId);

            var followersViewModel = followers.Select(f => new UserResponse
            {
                Id = f.Id,
                Email = f.Email,
                Username = f.Username,
                ProfilePictureUrl = f.ProfilePictureUrl
            }).ToList();

            return Response<IReadOnlyList<UserResponse>>.Success(followersViewModel);
        }
    }
}
