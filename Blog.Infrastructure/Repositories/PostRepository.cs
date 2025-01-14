using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post, int>, IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task AddCommentAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

    }
}
