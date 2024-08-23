using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post, int>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context) { }

    }
}
