using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IReadOnlyList<Comment>> GetPostComments(int postId)
        {
            return await _dbContext.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }
    }
}
