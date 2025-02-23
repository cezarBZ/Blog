using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetLoggedUserQueryHandler : IRequestHandler<GetLoggedUserQuery, Response<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        public GetLoggedUserQueryHandler(IUserRepository userRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
        }
        public async Task<Response<UserResponse>> Handle(GetLoggedUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId().Value;
            var user = await _userRepository.GetByIdAsync(userId);

            var userViewModel = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return Response<UserResponse>.Success(userViewModel);
        }
    }
}
