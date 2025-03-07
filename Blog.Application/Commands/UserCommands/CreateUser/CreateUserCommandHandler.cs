using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;

namespace Blog.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<SignupResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<Response<SignupResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var passwordHash = _authService.ComputeSha256Hash(request.Password);


                var user = new User(request.Username, request.Email, passwordHash, true, request.Role);
                var token = _authService.GenerateJwtToken(user.Email, user.Role, user.Id);

                await _userRepository.AddAsync(user);
                await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                var response = new SignupResponse(user.Email, token, user.Username, user.Role.ToString());

                return Response<SignupResponse>.Success(response, "Usuário criado com sucesso.");

            }
            catch (System.Exception error)
            {

                return Response<SignupResponse>.Failure(error.Message);
            }
        }
    }
}
