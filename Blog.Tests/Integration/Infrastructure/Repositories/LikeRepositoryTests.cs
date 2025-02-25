using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Blog.Tests.Integration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Integration.Infrastructure.Repositories
{
    public class LikeRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public LikeRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetLikeByUserIdAndTargetIdAsync_Should_Return_Like_If_Exists()
        {
            using (var context = new TestDbContext(_options))
            {
                var user = new User(1, "John Dow", "email", "hash", true, "user", "path");
                var postId = 1;
                var like = new Like(postId, user.Id, LikeTargetType.Post);

                await context.Users.AddAsync(user);
                await context.Likes.AddAsync(like);
                await context.SaveChangesAsync();

                var repository = new LikeRepository(context);

                var result = await repository.GetLikeByUserIdAndTargetIdAsync(1, 1, LikeTargetType.Post);

                Assert.NotNull(result);
                Assert.Equal(1, result.UserId);
                Assert.Equal(1, result.TargetId);
                Assert.Equal(LikeTargetType.Post, result.TargetType);
            }
        }

        [Fact]
        public async Task GetLikeByUserIdAndTargetIdAsync_Should_Return_Null_If_Like_Does_Not_Exist()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new LikeRepository(context);

                var result = await repository.GetLikeByUserIdAndTargetIdAsync(999, 999, LikeTargetType.Post); // Like que não existe

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetPostLikes_Should_Return_Likes_For_Specific_Post()
        {
            using (var context = new TestDbContext(_options))
            {
                var user1 = new User(1, "John Dow", "email", "hash", true, "user", "path");
                var user2 = new User(2, "Johnie Dow", "email", "hash", true, "user", "path");

                var likes = new List<Like>
                {
                    new Like(1, 1, LikeTargetType.Post),
                    new Like(1, 2, LikeTargetType.Post),
                    new Like(2, 1, LikeTargetType.Post)
                };

                await context.Users.AddRangeAsync(user1, user2);
                await context.Likes.AddRangeAsync(likes);
                await context.SaveChangesAsync();

                var repository = new LikeRepository(context);

                var result = await repository.GetPostLikes(1);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.All(result, like => Assert.Equal(1, like.TargetId));
            }
        }

        [Fact]
        public async Task GetPostLikes_Should_Return_Empty_List_If_No_Likes_Exist_For_Post()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new LikeRepository(context);

                var result = await repository.GetPostLikes(1);

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

    }
}
