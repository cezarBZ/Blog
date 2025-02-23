using Blog.Application.Queries.UserQueries;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Queries.UserQueries
{
    public class GetLoggedUserQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUserContextService> _userContextService;

        public GetLoggedUserQueryHandlerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _userContextService = new Mock<IUserContextService>();
        }

        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            var user = new User("email", "username", "password", true, "User", "url");

            _userContextService
                .Setup(service => service.GetUserId())
                .Returns(user.Id);
            _userRepository
                .Setup(repo => repo.GetByIdAsync(user.Id))
                .ReturnsAsync(user);

            var handler = new GetLoggedUserQueryHandler(_userRepository.Object, _userContextService.Object);
            var query = new GetLoggedUserQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(user.Id, result.Data.Id);

            _userRepository.Verify(repo => repo.GetByIdAsync(user.Id), Times.Once);
        }
    }
}
