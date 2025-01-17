using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using MediatR;

namespace Blog.Application.Commands.CreateUser
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            var user = new User(request.Username, request.Email, passwordHash, true, request.Role);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
