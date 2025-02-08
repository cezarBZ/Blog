using Blog.Application.Responses;
using Blog.Application.ViewModels;
using Blog.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Blog.Application.Queries.UserQueries
{
    public class GetFollowedQueryHandler : IRequestHandler<GetFollowedQuery, Response<IReadOnlyList<UserViewModel>>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowedQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<IReadOnlyList<UserViewModel>>> Handle(GetFollowedQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Response<IReadOnlyList<UserViewModel>>.NotFound("Usuário não encontrado.");
            }

            var followed = await _userRepository.GetFollowedAsync(request.UserId);

            var followedViewModel = followed.Select(f => new UserViewModel
            {
                Id = f.Id,
                Email = f.Email,
                Username = f.Username,
            }).ToList();

            return Response<IReadOnlyList<UserViewModel>>.Success(followedViewModel);
        }
    }
}
