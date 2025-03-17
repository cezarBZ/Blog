using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Blog.Tests.Integration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Integration.Infrastructure.Repositories
{
    public class PostRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public PostRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Post_With_Comments()
        {
            using (var context = new TestDbContext(_options))
            {
                var user = new User(1, "username", "email", "password", true, UserRole.User);
                var post = new Post(1, "Test Post", "conteúdo", "path", 1);
                post.User = user;
                var comments = new List<Comment>
                {
                    new Comment("conteúdo", post.Id, 2),
                    new Comment("Teste", post.Id, 3)
                };

                await context.Posts.AddAsync(post);
                await context.Comments.AddRangeAsync(comments);
                await context.SaveChangesAsync();

                var repository = new PostRepository(context);

                var result = await repository.GetByIdAsync(1);

                Assert.NotNull(result);
                Assert.Equal("Test Post", result.Title);
                Assert.Equal(2, result.Comments.Count);
            }
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_Post_Does_Not_Exist()
        {
            // Arrange
            using (var context = new TestDbContext(_options))
            {
                var repository = new PostRepository(context);

                var result = await repository.GetByIdAsync(999);

                Assert.Null(result);
            }
        }
    }
}
