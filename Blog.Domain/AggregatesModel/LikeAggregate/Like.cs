using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.LikeAggregate
{
    public class Like : Entity<int>, IAggregateRoot
    {
        public Like(int targetId, int userId, LikeTargetType targetType)
        {
            TargetId = targetId;
            UserId = userId;
            TargetType = targetType;
        }

        public int TargetId { get; private set; }
        public LikeTargetType TargetType { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime LikedAt { get; private set; } = DateTime.UtcNow;
    }
}
