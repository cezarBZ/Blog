using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class LikeRepository : Repository<Like, int>, ILikeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LikeRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Like> GetLikeByUserIdAndPostIdAsync(int userId, int postId)
        {
            return await _dbContext.Likes
                .Where(l => l.UserId == userId && l.PostId == postId)
                .FirstOrDefaultAsync();
        }
    }
}
