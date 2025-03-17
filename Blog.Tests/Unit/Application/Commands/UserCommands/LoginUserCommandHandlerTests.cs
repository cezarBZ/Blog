using Blog.Application.Commands.UserCommands.LoginUser;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Auth;
using Moq;

namespace Blog.Tests.Unit.Application.Commands.UserCommands
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthService> _authServiceMock;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthService>();
        }

        [Fact]
        public async Task Handle_ShouldLoginUser_WhenUserIsLoggedIn()
        {
            var user = new User(1, "Username", "Email", "Password", true, UserRole.User);
            var command = new LoginUserCommand {Email = "Email", Password = "Password" };

            _authServiceMock.Setup(x => x.ComputeSha256Hash(It.IsAny<string>())).Returns("Password");
            _authServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<string>(), It.IsAny<UserRole>(), It.IsAny<int>())).Returns("Token");
            _userRepositoryMock.Setup(x => x.GetUserByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Verifiable();
            _userRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new LoginUserCommandHandler(_authServiceMock.Object, _userRepositoryMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Email", result.Email);
            Assert.Equal("Token", result.Token);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserNotFound()
        {
            var command = new LoginUserCommand { Email = "Email", Password = "Password" };

            _authServiceMock.Setup(x => x.ComputeSha256Hash(It.IsAny<string>())).Returns("Password");
            _userRepositoryMock.Setup(x => x.GetUserByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((User?)null);

            var handler = new LoginUserCommandHandler(_authServiceMock.Object, _userRepositoryMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
        }

    }
}
