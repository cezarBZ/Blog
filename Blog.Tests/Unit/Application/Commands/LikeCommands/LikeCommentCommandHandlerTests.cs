using Blog.Application.Commands.LikeCommands.LikeComment;
using Blog.Application.Commands.LikeCommands.LikePost;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Moq;
using System.ComponentModel.Design;

namespace Blog.Tests.Unit.Application.Commands.LikeCommands
{
    public class LikeCommentCommandHandlerTests
    {
        private readonly Mock<ILikeRepository> _likeRepositoryMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly LikeCommentCommandHandler _handler;
        public LikeCommentCommandHandlerTests()
        {
            _likeRepositoryMock = new Mock<ILikeRepository>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new LikeCommentCommandHandler(_likeRepositoryMock.Object, _userContextServiceMock.Object, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldLikeCommentAndReturnSuccess()
        {
            var postId = 1;
            var commentId = 3;
            var userId = 2;

            var comment = new Comment("titulo", postId, userId);
            var command = new LikeCommentCommand { commentId = commentId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _commentRepositoryMock.Setup(p => p.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, commentId, LikeTargetType.Comment)).ReturnsAsync((Like?)null);
            _likeRepositoryMock.Setup(l => l.AddAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);
            _commentRepositoryMock.Setup(p => p.Update(It.IsAny<Comment>())).Verifiable();
            _likeRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, comment.LikeCount);
            Assert.Equal("Comentário curtido com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_CommentNotFound_ShouldLReturnNotFound()
        {
            var postId = 1;
            var commentId = 1;
            var userId = 2;

            var command = new LikeCommentCommand { commentId = commentId };

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _commentRepositoryMock.Setup(p => p.GetByIdAsync(postId)).ReturnsAsync(default(Comment));


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Comment não encontrado.", result.Message);
        }

        [Fact]
        public async Task Handle_CommentAlreadyHasBeenLiked_ShouldLReturnFailure()
        {
            var postId = 1;
            var commentId = 1;
            var userId = 2;

            var command = new LikeCommentCommand { commentId = commentId };
            var comment = new Comment("titulo", postId, userId);

            var like = new Like(postId, userId, LikeTargetType.Post);

            _userContextServiceMock.Setup(u => u.GetUserId()).Returns(userId);
            _commentRepositoryMock.Setup(p => p.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _likeRepositoryMock.Setup(l => l.GetLikeByUserIdAndTargetIdAsync(userId, commentId, LikeTargetType.Comment)).ReturnsAsync(like);


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Você já curtiu este comentário.", result.Message);
        }
    }
}
