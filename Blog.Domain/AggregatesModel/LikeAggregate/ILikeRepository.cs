using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.LikeAggregate
{
    public interface ILikeRepository : IRepository<Like, int>
    {
        Task<Like> GetLikeByUserIdAndPostIdAsync(int userId, int postId);
    }
}
