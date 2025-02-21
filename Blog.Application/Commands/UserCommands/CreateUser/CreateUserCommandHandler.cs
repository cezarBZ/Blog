using Blog.Application.Responses;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using Blog.Infrastructure.Services.BlobStorage;
using MediatR;

namespace Blog.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IFileStorageService fileStorageService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService, IFileStorageService fileStorageService)
        {
            _userRepository = userRepository;
            _authService = authService;
            this.fileStorageService = fileStorageService;
        }
        public async Task<Response<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var passwordHash = _authService.ComputeSha256Hash(request.Password);
                string coverImageUrl = null;
                if (request.ProfilePicture != null)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ProfilePicture.FileName)}";
                    coverImageUrl = await fileStorageService.UploadFileAsync(request.ProfilePicture, fileName);
                }

                var user = new User(request.Username, request.Email, passwordHash, true, request.Role, coverImageUrl);

                await _userRepository.AddAsync(user);
                await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return Response<int>.Success(user.Id, "Usuário criado com sucesso.");

            }
            catch (System.Exception error)
            {

                return Response<int>.Failure(error.Message);
            }
        }
    }
}
