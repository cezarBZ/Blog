using Blog.Application.Commands.CommentCommands.UpdateComment;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands.CommentCommands
{
    public class UpdateCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly UpdateCommentCommandHandler _handler;

        public UpdateCommentCommandHandlerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new UpdateCommentCommandHandler(_userContextServiceMock.Object, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateCommentAndReturnSuccess()
        {
            var commentId = 1;
            var postId = 2;
            var user = new User("trabson", "trabson@example.com", "xxxxxxxx", true, "User", "");
            var comment = new Comment("Conteúdo do Comentário", postId, user.Id);
            var content = "Comentário atualizado";
            var command = new UpdateCommentCommand { CommentId = commentId, Content = content };

            _commentRepositoryMock.Setup(x => x.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _commentRepositoryMock.Setup(x => x.Update(It.IsAny<Comment>())).Verifiable();
            _userContextServiceMock.Setup(x => x.GetUserAsync()).ReturnsAsync(user);
            _commentRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            Assert.Equal("Comentário editado com sucesso", result.Message);
            Assert.Equal("Comentário atualizado", comment.Content);
            _commentRepositoryMock.Verify(x => x.Update(It.IsAny<Comment>()), Times.Once);
            _commentRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CommentNotFound_ShouldReturnNotFound()
        {
            var commentId = 1;
            var user = new User("trabson", "trabson@example.com", "xxxxxxxx", true, "User", "");
            var content = "Comentário atualizado";

            var command = new UpdateCommentCommand { CommentId = commentId, Content = content };

            _commentRepositoryMock.Setup(x => x.GetByIdAsync(commentId)).ReturnsAsync(default(Comment));
            _userContextServiceMock.Setup(x => x.GetUserAsync()).ReturnsAsync(user);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);

            Assert.Equal("Comentário não encontrado", result.Message);
            _commentRepositoryMock.Verify(x => x.GetByIdAsync(commentId), Times.Once);
            _userContextServiceMock.Verify(x => x.GetUserAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotCommentOwner_ShouldReturnFailure()
        {
            var commentId = 1;
            var postId = 2;
            var userId = 766;
            var user = new User("trabson", "trabson@example.com", "xxxxxxxx", true, "User", "");
            var comment = new Comment("Conteúdo do Comentário", postId, userId);
            var content = "Comentário atualizado";
            var command = new UpdateCommentCommand { CommentId = commentId, Content = content };

            _commentRepositoryMock.Setup(x => x.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _userContextServiceMock.Setup(x => x.GetUserAsync()).ReturnsAsync(user);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);

            Assert.Equal("Você não pode editar um comentário de outro usuário", result.Message);
            _commentRepositoryMock.Verify(x => x.GetByIdAsync(commentId), Times.Once);
            _userContextServiceMock.Verify(x => x.GetUserAsync(), Times.Once);

        }
    }
}
