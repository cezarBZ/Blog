using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public override async Task<Post> GetByIdAsync(int id)
        {
            return await _dbContext.Posts
                .Include(p => p.Comments) // Inclui os comentários
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
