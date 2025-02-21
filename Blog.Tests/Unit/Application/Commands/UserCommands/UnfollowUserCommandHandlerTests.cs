using Blog.Application.Commands.UserCommands.Unfollow;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands.UserCommands
{
    public class UnfollowUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;

        public UnfollowUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
        }

        [Fact]
        public async Task Handle_ShouldUnfollowUser_WhenUserIsUnfollowed()
        {
            var followerId = 1;
            var followedId = 2;
            var follower = new User(followerId, "Username", "Email", "Password", true, "Role", "CoverImageUrl");

            var followed = new User(followedId, "Username", "Email", "Password", true, "Role", "CoverImageUrl");

            follower.Follow(followed);

            var command = new UnfollowUserCommand(followedId);

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _userRepositoryMock.Setup(x => x.GetByIdWithFollowedAsync(followerId)).ReturnsAsync(follower);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(followedId)).ReturnsAsync(followed);
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Verifiable();
            _userRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new UnfollowUserCommandHandler(_userRepositoryMock.Object, _userContextServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(0, followed.FollowersCount);
            Assert.Equal("Successfully unfollowed the user.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFollowerNotFound()
        {
            var command = new UnfollowUserCommand(1);

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _userRepositoryMock.Setup(x => x.GetByIdWithFollowedAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var handler = new UnfollowUserCommandHandler(_userRepositoryMock.Object, _userContextServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Follower not found.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserToUnfollowNotFound()
        {
            var command = new UnfollowUserCommand(1);

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _userRepositoryMock.Setup(x => x.GetByIdWithFollowedAsync(It.IsAny<int>())).ReturnsAsync(new User(1, "Username", "Email", "Password", true, "Role", "CoverImageUrl"));
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var handler = new UnfollowUserCommandHandler(_userRepositoryMock.Object, _userContextServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User to unfollow not found.", result.Message);
        }
    }
}
