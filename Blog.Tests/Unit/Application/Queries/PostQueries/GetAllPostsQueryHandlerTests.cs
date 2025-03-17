using Blog.Application.Queries.PostQueries.GetAllPosts;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Moq;

namespace Blog.Tests.Unit.Application.Queries.PostQueries
{
    public class GetAllPostsQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepository;

        public GetAllPostsQueryHandlerTests()
        {
            _postRepository = new Mock<IPostRepository>();
        }

        [Fact]
        public async Task Handle_ShouldReturnPosts_WhenPostsExist()
        {
            var post1 = new Post("Title 1", "Content 1", "url", 1);
            var user = new User(1, "username", "email", "password", true, UserRole.User);
            var post2 = new Post("Title 2", "Content 2", "url", 1);
            post1.User = user;
            post2.User = user;
            var posts = new List<Post>
            {
                post1, post2
            };

            _postRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>()))
                .ReturnsAsync(posts);

            var handler = new GetAllPostsQueryHandler(_postRepository.Object);
            var query = new GetAllPostsQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(posts.Count, result.Data.Count);

            _postRepository.Verify(repo => repo.GetAllAsync(It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostsDoNotExist()
        {
            var posts = new List<Post>();

            _postRepository
                .Setup(repo => repo.GetAllAsync(It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>()))
                .ReturnsAsync(posts);

            var handler = new GetAllPostsQueryHandler(_postRepository.Object);
            var query = new GetAllPostsQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("No posts found", result.Message);

            _postRepository.Verify(repo => repo.GetAllAsync(It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>()), Times.Once);
        }
    }
}
