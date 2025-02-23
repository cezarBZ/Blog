using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Integration.Infrastructure.Data
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
