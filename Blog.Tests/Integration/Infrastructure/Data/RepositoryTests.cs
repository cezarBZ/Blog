using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Integration.Infrastructure.Data
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly TestDbContext _context;

        public RepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TestDbContext(_options);
        }


        [Fact]
        public async Task AddAsync_Should_Add_Entity_To_Database()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);
                var user = new User(1, "John Doe", "email", "senha", true, UserRole.User);

                await repository.AddAsync(user);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal("John Doe", result.Username);
            }
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Entity_If_Exists()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);
                var user = new User(1, "John Doe", "email", "senha", true, UserRole.User);
                await repository.AddAsync(user);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(1);

                Assert.NotNull(result);
                Assert.Equal("John Doe", result.Username);
            }
        }

        [Fact]
        public async Task GetById_Should_Return_Entity_If_Exists()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);
                var user = new User(1, "John Doe", "email", "senha", true, UserRole.User);
                await repository.AddAsync(user);
                await context.SaveChangesAsync();

                var result = repository.GetById(1);

                Assert.NotNull(result);
                Assert.Equal("John Doe", result.Username);
            }
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_Entity_Does_Not_Exist()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);

                var result = await repository.GetByIdAsync(999);

                Assert.Null(result);
            }
        }

        [Fact]
        public void GetById_Should_Return_Null_If_Entity_Does_Not_Exist()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);

                var result = repository.GetById(999); 

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task Update_Should_Modify_Existing_Entity()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);
                var user = new User(1, "John Doe", "email", "senha", true, UserRole.User);
                await repository.AddAsync(user);
                await context.SaveChangesAsync();

                user.Username = "Jane Doe";
                repository.Update(user);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal("Jane Doe", result.Username);
            }
        }

        [Fact]
        public async Task Delete_Should_Remove_Entity_From_Database()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);
                var user = new User(1, "John Doe", "email", "senha", true, UserRole.User);
                await repository.AddAsync(user);
                await context.SaveChangesAsync();

                repository.Delete(user);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(1);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Entities()
        {
            using (var context = new TestDbContext(_options))
            {
                var repository = new Repository<User, int>(context);
                await repository.AddAsync(new User(1, "John Doe", "email", "senha", true, UserRole.User));
                await repository.AddAsync(new User(2, "Jane Doe", "email", "senha", true, UserRole.User));
                await context.SaveChangesAsync();

                var result = await repository.GetAllAsync();

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
            }
        }

    }
}
