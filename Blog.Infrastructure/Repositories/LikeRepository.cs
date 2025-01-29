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

        public async Task<Like> GetLikeByUserIdAndTargetIdAsync(int userId, int targetId, LikeTargetType targetType)
        {
            return await _dbContext.Likes
                .Where(l => l.UserId == userId && l.TargetType == targetType && l.TargetId == targetId)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Like>> GetPostLikes(int postId)
        {
            return await _dbContext.Likes
                .Where(l => l.TargetType == LikeTargetType.Post && l.TargetId == postId)
                .ToListAsync();
        }
    }
}
