using Blog.Application.Commands.LikeCommands.DislikeComment;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands.LikeCommands
{
    public class DislikeCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<ILikeRepository> _likeRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly DislikeCommentCommandHandler _handler;
        public DislikeCommentCommandHandlerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _likeRepositoryMock = new Mock<ILikeRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new DislikeCommentCommandHandler(_likeRepositoryMock.Object, _userContextServiceMock.Object, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldDislikePostAndReturnSuccess()
        {
            var postId = 1;
            var userId = 2;
            var commentId = 3;

            var comment = new Comment("blablabla", postId, userId);
            var command = new DislikeCommentCommand { CommentId = commentId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _commentRepositoryMock.Setup(p => p.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, commentId, LikeTargetType.Comment)).ReturnsAsync(new Like(userId, commentId, LikeTargetType.Comment));
            _likeRepositoryMock.Setup(l => l.Delete(It.IsAny<Like>())).Verifiable();
            _commentRepositoryMock.Setup(p => p.Update(It.IsAny<Comment>())).Verifiable();
            _likeRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(0, comment.LikeCount);
            Assert.Equal("Comment descurtido com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_PostNotFound_ShouldReturnFailure()
        {
            var postId = 1;
            var userId = 2;
            var commentId = 3;

            var comment = new Comment("blablabla", postId, userId);
            var command = new DislikeCommentCommand { CommentId = commentId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _commentRepositoryMock.Setup(p => p.GetByIdAsync(commentId)).ReturnsAsync((Comment?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Comment não encontrado.", result.Message);
        }

        [Fact]
        public async Task Handle_UserDidNotLikePost_ShouldReturnFailure()
        {
            var postId = 1;
            var userId = 2;
            var commentId = 3;

            var comment = new Comment("blablabla", postId, userId);
            var command = new DislikeCommentCommand { CommentId = commentId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _commentRepositoryMock.Setup(p => p.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, commentId, LikeTargetType.Comment)).ReturnsAsync((Like?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Você ainda não curtiu esse comment.", result.Message);
        }

    }
}
