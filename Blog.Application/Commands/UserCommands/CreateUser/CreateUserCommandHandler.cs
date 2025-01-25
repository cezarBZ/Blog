using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using MediatR;

namespace Blog.Application.Commands.UserCommands.CreateUser
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<Response<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            var user = new User(request.Username, request.Email, passwordHash, true, request.Role);

            await _userRepository.AddAsync(user);

            return Response<int>.Success(user.Id, "Usuário criado com sucesso.");
        }
    }
}
