using Blog.Application.Commands.LikeCommands.LikePost;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands
{
    public class LikePostCommandHandlerTests
    {
        private readonly Mock<ILikeRepository> _likeRepositoryMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly LikePostCommandHandler _handler;
        public LikePostCommandHandlerTests()
        {
            _likeRepositoryMock = new Mock<ILikeRepository>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new LikePostCommandHandler(_likeRepositoryMock.Object, _userContextServiceMock.Object, _postRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldLikePostAndReturnSuccess()
        {
            var postId = 1;
            var userId = 2;

            var post = new Post("titulo", "blablabla", "", userId);
            var command = new LikePostCommand { postId = postId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(post);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, postId, LikeTargetType.Post)).ReturnsAsync((Like?)null);
            _likeRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);
            _postRepositoryMock.Setup(p => p.Update(It.IsAny<Post>())).Verifiable();
            _likeRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, post.LikeCount);
            Assert.Equal("Post curtido com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_UserNotLoggedIn_ShouldReturnFailure()
        {
            var postId = 1;

            var post = new Post("titulo", "blablabla", "", 2);
            var command = new LikePostCommand { postId = postId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns((int?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não logado.", result.Message);
        }

        [Fact]
        public async Task Handle_PostNotFound_ShouldLReturnNotFound()
        {
            var postId = 1;
            var userId = 2;

            var command = new LikePostCommand { postId = postId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(default(Post));


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Post não encontrado.", result.Message);
        }

        [Fact]
        public async Task Handle_PostAlreadyHasBeenLiked_ShouldLReturnFailure()
        {
            var postId = 1;
            var userId = 2;

            var command = new LikePostCommand { postId = postId };
            var post = new Post("titulo", "blablabla", "", 2);
            var like = new Like(postId, userId, LikeTargetType.Post);

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(post);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, postId, LikeTargetType.Post)).ReturnsAsync(like);


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Você já curtiu este post.", result.Message);
        }
    }
}
