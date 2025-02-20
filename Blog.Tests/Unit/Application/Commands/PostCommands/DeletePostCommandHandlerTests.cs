using Blog.Application.Commands.PostCommands.DeletePost;
using Blog.Domain.AggregatesModel.PostAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Commands.PostCommands
{
    public class DeletePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        public DeletePostCommandHandlerTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
        }

        [Fact]
        public async Task Handle_ShouldDeletePost_WhenPostIsDeleted()
        {
            var postId = 1;
            var post = new Post("Title", "Content", "CoverImage", 1);
            var command = new DeletePostCommand(postId);

            _postRepositoryMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
            _postRepositoryMock.Setup(x => x.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new DeletePostCommandHandler(_postRepositoryMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Post deleted succesfuly", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenPostIsNotFound()
        {
            var postId = 1;
            var command = new DeletePostCommand(postId);

            _postRepositoryMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(default(Post));

            var handler = new DeletePostCommandHandler(_postRepositoryMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Post not found.", result.Message);
        }
    }
}
