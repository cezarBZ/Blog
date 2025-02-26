using Blog.Application.Queries.UserQueries;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Queries.UserQueries
{
    public class GetFollowedQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public GetFollowedQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnFollowedUsers_WhenUsersExist()
        {
            var userFollower = new User("email", "username", "password", true, UserRole.User, "url");
            var user1 = new User("email1", "username1", "password1", true, UserRole.User, "url");
            var user2 = new User("email2", "username2", "password2", true, UserRole.User, "url");
            var followed = new List<User>
            {
                user1, user2
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userFollower.Id))
                .ReturnsAsync(user1);

            _userRepositoryMock
                .Setup(repo => repo.GetFollowedAsync(It.IsAny<int>()))
                .ReturnsAsync(followed);

            var handler = new GetFollowedQueryHandler(_userRepositoryMock.Object);
            var query = new GetFollowedQuery(userFollower.Id);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(followed.Count, result.Data.Count);

            _userRepositoryMock.Verify(repo => repo.GetFollowedAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUsersDoNotExist()
        {
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((User?)null);

            var handler = new GetFollowedQueryHandler(_userRepositoryMock.Object);
            var query = new GetFollowedQuery(1);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não encontrado.", result.Message);
        }
    }
}
