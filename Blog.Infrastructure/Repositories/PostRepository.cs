using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
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

        public async Task AddLikeAsync(Like like)
        {
            await _dbContext.Likes.AddAsync(like);
        }

        public override async Task<Post> GetByIdAsync(int id)
        {
            return await _dbContext.Posts
                .Include(p => p.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Like> GetLikeByUserIdAndPostIdAsync(int userId, int postId)
        {
            return await _dbContext.Likes
                .Where(l => l.UserId == userId && l.PostId == postId)
                .FirstOrDefaultAsync();
        }
    }
}
