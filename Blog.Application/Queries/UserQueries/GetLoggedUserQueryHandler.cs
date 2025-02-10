using Blog.Application.Responses;
using Blog.Application.Services;
using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetLoggedUserQueryHandler : IRequestHandler<GetLoggedUserQuery, Response<UserViewModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        public GetLoggedUserQueryHandler(IUserRepository userRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
        }
        public async Task<Response<UserViewModel>> Handle(GetLoggedUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId().Value;
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                return Response<UserViewModel>.Failure("User not found.");
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return Response<UserViewModel>.Success(userViewModel);
        }
    }
}
