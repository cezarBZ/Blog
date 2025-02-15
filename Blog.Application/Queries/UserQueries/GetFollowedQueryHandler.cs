using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowedQueryHandler : IRequestHandler<GetFollowedQuery, Response<IReadOnlyList<UserResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowedQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<IReadOnlyList<UserResponse>>> Handle(GetFollowedQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Response<IReadOnlyList<UserResponse>>.NotFound("Usuário não encontrado.");
            }

            var followed = await _userRepository.GetFollowedAsync(request.UserId);

            var followedViewModel = followed.Select(f => new UserResponse
            {
                Id = f.Id,
                Email = f.Email,
                Username = f.Username,
                ProfilePictureUrl = f.ProfilePictureUrl
            }).ToList();

            return Response<IReadOnlyList<UserResponse>>.Success(followedViewModel);
        }
    }
}
