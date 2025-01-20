using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.PostAggregate
{
    public class Like : Entity<int>
    {
        public Like(int postId, int userId)
        {
            PostId = postId;
            UserId = userId;
        }

        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
