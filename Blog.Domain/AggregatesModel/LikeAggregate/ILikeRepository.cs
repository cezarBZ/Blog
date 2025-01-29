using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.LikeAggregate
{
    public interface ILikeRepository : IRepository<Like, int>
    {
        Task<Like> GetLikeByUserIdAndTargetIdAsync(int userId, int targetId, LikeTargetType targetType);
        Task<IReadOnlyList<Like>> GetPostLikes(int postId);
    }
}
