using Blog.Application.Commands.UserCommands.Follow;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Models;
using Moq;
using System.Reflection;

namespace Blog.Tests.Unit.Application.Commands.UserCommands
{
    public class FollowUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;

        public FollowUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
        }

        [Fact]
        public async Task Handle_ShouldFollowUser_WhenUserIsFollowed()
        {
            var followerId = 1;
            var followedId = 2;
            var follower = new User(followerId, "Username", "Email", "Password", true, UserRole.User);

            var followed = new User(followedId, "Username", "Email", "Password", true, UserRole.User);

            var command = new FollowUserCommand(followedId);

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(followerId)).ReturnsAsync(follower);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(followedId)).ReturnsAsync(followed);
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Verifiable();
            _userRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new FollowUserCommandHandler(_userRepositoryMock.Object, _userContextServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, followed.FollowersCount);
            Assert.Equal("Successfully followed the user.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFollowerNotFound()
        {
            var command = new FollowUserCommand(1);

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var handler = new FollowUserCommandHandler(_userRepositoryMock.Object, _userContextServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Follower not found.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserToFollowNotFound()
        {
            var followerId = 1;
            var followedId = 2;
            var follower = new User("Username", "Email", "Password", true, UserRole.User);
            var command = new FollowUserCommand(followedId);

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(1);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(followerId)).ReturnsAsync(follower);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(followedId)).ReturnsAsync((User?)null);

            var handler = new FollowUserCommandHandler(_userRepositoryMock.Object, _userContextServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User to follow not found.", result.Message);
        }
    }
}