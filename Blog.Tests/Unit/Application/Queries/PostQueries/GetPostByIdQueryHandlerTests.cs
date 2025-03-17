using Blog.Application.Queries.PostQueries.GetPostById;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Queries.PostQueries
{
    public class GetPostByIdQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository;
        public GetPostByIdQueryHandlerTests()
        {
            _postRepository = new Mock<IPostRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnPost_WhenPostExists()
        {
            var user = new User(1, "username", "email", "password", true, UserRole.User);
            var post = new Post("Title 1", "Content 1", "url", 1);
            post.User = user;
            _postRepository
                .Setup(repo => repo.GetByIdAsync(post.Id))
                .ReturnsAsync(post);

            var handler = new GetPostByIdQueryHandler(_postRepository.Object);
            var query = new GetPostByIdQuery(post.Id);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(post.Id, result.Data.Id);

            _postRepository.Verify(repo => repo.GetByIdAsync(post.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostDoesNotExist()
        {
            var postId = 1;

            _postRepository
                .Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync((Post?)null);

            var handler = new GetPostByIdQueryHandler(_postRepository.Object);
            var query = new GetPostByIdQuery(postId);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Post not found", result.Message);

            _postRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
        }
    }
}
