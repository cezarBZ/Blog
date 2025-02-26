using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Blog.Tests.Unit.Application.Services
{
    public class UserContextServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserContextService _userContextService;

        public UserContextServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userContextService = new UserContextService(_httpContextAccessorMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserAsync_WhenUserIdClaimExists_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var user = new User(userId, "John Doe", "email", "senha", true, UserRole.User, "path");

            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userContextService.GetUserAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("John Doe", result.Username);
        }

        [Fact]
        public async Task GetUserAsync_WhenUserIdClaimDoesNotExist_ReturnsNull()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = await _userContextService.GetUserAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserId_WhenUserIdClaimExists_ReturnsUserId()
        {
            // Arrange
            var userId = 1;

            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _userContextService.GetUserId();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result);
        }

        [Fact]
        public void GetUserId_WhenUserIdClaimDoesNotExist_ReturnsNull()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _userContextService.GetUserId();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserId_WhenUserIdClaimIsInvalid_ReturnsNull()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim("userId", "invalidId")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _userContextService.GetUserId();

            // Assert
            Assert.Null(result);
        }

    }
}
