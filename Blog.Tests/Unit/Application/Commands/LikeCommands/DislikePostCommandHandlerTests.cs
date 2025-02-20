using Blog.Application.Services;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Application.Commands.LikeCommands.DislikePost;
using Moq;

namespace Blog.Tests.Unit.Application.Commands.LikeCommands
{
    public class DislikePostCommandHandlerTests
    {
        private readonly Mock<ILikeRepository> _likeRepositoryMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly DislikePostCommandHandler _handler;
        public DislikePostCommandHandlerTests()
        {
            _likeRepositoryMock = new Mock<ILikeRepository>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new DislikePostCommandHandler(_likeRepositoryMock.Object, _userContextServiceMock.Object, _postRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldDislikePostAndReturnSuccess()
        {
            var postId = 1;
            var userId = 2;

            var post = new Post("titulo", "blablabla", "", userId);
            var command = new DislikePostCommand { PostId = postId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(post);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, postId, LikeTargetType.Post)).ReturnsAsync(new Like(userId, postId, LikeTargetType.Post));
            _likeRepositoryMock.Setup(l => l.Delete(It.IsAny<Like>())).Verifiable();
            _postRepositoryMock.Setup(p => p.Update(It.IsAny<Post>())).Verifiable();
            _likeRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(0, post.LikeCount);
            Assert.Equal("Post descurtido com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_PostNotFound_ShouldReturnFailure()
        {
            var postId = 1;
            var userId = 2;

            var command = new DislikePostCommand { PostId = postId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(default(Post));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Post não encontrado.", result.Message);
        }

        [Fact]
        public async Task Handle_UserDidNotLikePost_ShouldReturnFailure()
        {
            var postId = 1;
            var userId = 2;

            var post = new Post("titulo", "blablabla", "", userId);
            var command = new DislikePostCommand { PostId = postId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(post);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, postId, LikeTargetType.Post)).ReturnsAsync((Like?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Você ainda não curtiu esse post.", result.Message);
        }
    }
}
