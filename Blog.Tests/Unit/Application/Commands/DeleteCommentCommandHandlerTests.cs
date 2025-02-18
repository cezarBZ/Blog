using Blog.Application.Commands.CommentCommands.DeleteComment;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands
{
    public class DeleteCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly DeleteCommentCommandHandler _handler;

        public DeleteCommentCommandHandlerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _handler = new DeleteCommentCommandHandler(_postRepositoryMock.Object, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldDeleteCommentAndReturnSuccess()
        {
            var commentId = 1;
            var postId = 1;
            var userId = 1;

            var command = new DeleteCommentCommand { CommentId = commentId };

            _commentRepositoryMock.Setup(x => x.GetByIdAsync(commentId)).ReturnsAsync(new Comment("Conteúdo do Comentário", postId, userId));
            _postRepositoryMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(new Post("Título do Post", "Conteúdo do Post", "", 1));
            _commentRepositoryMock.Setup(x => x.Delete(It.IsAny<Comment>())).Verifiable();
            _postRepositoryMock.Setup(x => x.Update(It.IsAny<Post>())).Verifiable();
            _commentRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Comentário deletado com sucesso", result.Message);
            _commentRepositoryMock.Verify(x => x.Delete(It.IsAny<Comment>()), Times.Once);
            _postRepositoryMock.Verify(x => x.Update(It.IsAny<Post>()), Times.Once);
            _commentRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CommentNotFound_ShouldReturnNotFound()
        {
            var commentId = 1;

            var command = new DeleteCommentCommand { CommentId = commentId };

            _commentRepositoryMock.Setup(x => x.GetByIdAsync(commentId)).ReturnsAsync(default(Comment));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("comment not found", result.Message);
        }
    }
}
