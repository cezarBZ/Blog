using Blog.Application.Commands.CommentCommands.CreateComment;
using Blog.Application.Services;
using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands
{
    public class CreateCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;
        private readonly CreateCommentCommandHandler _handler;

        public CreateCommentCommandHandlerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _handler = new CreateCommentCommandHandler(_userContextServiceMock.Object, _commentRepositoryMock.Object, _postRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateCommentAndReturnSuccess()
        {
            var userId = 1;
            var postId = 1;
            var content = "Novo comentário";

            var command = new CreateCommentCommand { Content = content, postId = postId };

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(new Post("Título do Post", "Conteúdo do Post", "", userId));
            _commentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);
            _postRepositoryMock.Setup(x => x.Update(It.IsAny<Post>())).Verifiable();
            _commentRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Comentário criado com sucesso.", result.Message);
            _commentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Comment>()), Times.Once);
            _postRepositoryMock.Verify(x => x.Update(It.IsAny<Post>()), Times.Once);
            _commentRepositoryMock.Verify(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotLoggedIn_ShouldReturnNotFound()
        {
            var command = new CreateCommentCommand { Content = "Novo comentário", postId = 1 };

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns((int?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não está logado.", result.Message);
        }

        [Fact]
        public async Task Handle_PostNotFound_ShouldReturnFailure()
        {
            var userId = 1;
            var postId = 1;
            var command = new CreateCommentCommand { Content = "Novo comentário", postId = postId };

            _userContextServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(default(Post));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Post não encontrado.", result.Message);
        }
    }
}
