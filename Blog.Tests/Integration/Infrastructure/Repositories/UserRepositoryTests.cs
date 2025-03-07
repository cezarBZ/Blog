using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Blog.Tests.Integration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Integration.Infrastructure.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordAsync_Should_Return_User_If_Exists()
        {
            using (var context = new TestDbContext(_options))
            {
                var user = new User(1, "Fulano", "test@example.com", "hashedpassword", true, UserRole.User);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var result = await repository.GetUserByEmailAndPasswordAsync("test@example.com", "hashedpassword");

                Assert.NotNull(result);
                Assert.Equal("test@example.com", result.Email);
                Assert.Equal("hashedpassword", result.PasswordHash);
            }
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordAsync_Should_Return_Null_If_User_Does_Not_Exist()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new UserRepository(context);

                var result = await repository.GetUserByEmailAndPasswordAsync("nonexistent@example.com", "wrongpassword");

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetByIdWithFollowedAsync_Should_Return_User_With_Followed()
        {
            using (var context = new TestDbContext(_options))
            {
                var user1 = new User(1, "Fulano", "email1", "hash", true, UserRole.User);
                var user2 = new User(2, "Fulano", "email2", "hash", true, UserRole.User);
                var user3 = new User(3, "Fulano", "email3", "hash", true, UserRole.User);

                user1.Following.Add(new UserFollower(1, 2));
                user1.Following.Add(new UserFollower(1, 3));

                await context.Users.AddRangeAsync(user1, user2, user3);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var result = await repository.GetByIdWithFollowedAsync(1);

                Assert.NotNull(result);
                Assert.Equal(2, result.Following.Count);
            }
        }

        [Fact]
        public async Task GetFollowersAsync_Should_Return_Followers_For_Specific_User()
        {
            using (var context = new TestDbContext(_options))
            {
                var user1 = new User(1, "Fulano 1", "email1", "hash", true, UserRole.User);
                var user2 = new User(2, "Fulano 2", "email2", "hash", true, UserRole.User);
                var user3 = new User(3, "Fulano 3", "email3", "hash", true, UserRole.User);

                user1.Followers.Add(new UserFollower(2, 1));
                user1.Followers.Add(new UserFollower(3, 1));

                await context.Users.AddRangeAsync(user1, user2, user3);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var result = await repository.GetFollowersAsync(1);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async Task GetFollowedAsync_Should_Return_Followed_For_Specific_User()
        {
            // Arrange
            using (var context = new TestDbContext(_options))
            {
                var user1 = new User(1, "Fulano", "email1", "hash", true, UserRole.User);
                var user2 = new User(2, "Fulano", "email1", "hash", true, UserRole.User);
                var user3 = new User(3, "Fulano", "email1", "hash", true, UserRole.User);

                user1.Following.Add(new UserFollower(1, 2));
                user1.Following.Add(new UserFollower(1, 3));

                await context.Users.AddRangeAsync(user1, user2, user3);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var result = await repository.GetFollowedAsync(1);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count); 
            }
        }
    }
}
