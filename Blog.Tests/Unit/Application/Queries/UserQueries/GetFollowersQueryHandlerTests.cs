using Blog.Application.Queries.UserQueries;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Queries.UserQueries
{
    public class GetFollowersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public GetFollowersQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnFollowers_WhenUsersExist()
        {
            var userFollowed = new User("email", "username", "password", true, UserRole.User, "url");
            var user1 = new User("email1", "username1", "password1", true, UserRole.User, "url");
            var user2 = new User("email2", "username2", "password2", true, UserRole.User, "url");
            var followers = new List<User>
            {
                user1, user2
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userFollowed.Id))
                .ReturnsAsync(user1);

            _userRepositoryMock
                .Setup(repo => repo.GetFollowersAsync(It.IsAny<int>()))
                .ReturnsAsync(followers);

            var handler = new GetFollowersQueryHandler(_userRepositoryMock.Object);
            var query = new GetFollowersQuery(userFollowed.Id);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(followers.Count, result.Data.Count);

            _userRepositoryMock.Verify(repo => repo.GetFollowersAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUsersDoNotExist()
        {
            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((User?)null);

            var handler = new GetFollowersQueryHandler(_userRepositoryMock.Object);
            var query = new GetFollowersQuery(1);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }
    }
}
