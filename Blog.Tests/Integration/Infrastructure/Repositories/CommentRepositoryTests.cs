using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Blog.Tests.Integration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Integration.Infrastructure.Repositories
{
    public class CommentRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public CommentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetPostComments_Should_Return_Comments_For_Specific_Post()
        {
            using (var context = new TestDbContext(_options))
            {
                var post = new Post(1, "Title", "Content", "path", 1);
                var user = new User(1, "Cézar", "email", "hash", true, "User", "path");

                var comments = new List<Comment>
                {
                    new Comment("content", post.Id, user.Id),
                    new Comment ("content", post.Id, user.Id),
                    new Comment ("content", 3, user.Id)
                };

                await context.Posts.AddAsync(post);
                await context.Users.AddAsync(user);
                await context.Comments.AddRangeAsync(comments);
                await context.SaveChangesAsync();

                var repository = new CommentRepository(context);

                var result = await repository.GetPostComments(1);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.All(result, comment => Assert.Equal(1, comment.PostId));
            }
        }

        [Fact]
        public async Task GetPostComments_Should_Return_Empty_List_If_No_Comments_Exist_For_Post()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new CommentRepository(context);

                var result = await repository.GetPostComments(1);

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
    }
}
