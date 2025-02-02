
namespace Blog.Domain.AggregatesModel.UserAggregate
{
    public class UserFollower
    {
        public UserFollower(int followerId, int followedId)
        {
            FollowerId = followerId;
            FollowedId = followedId;
            FollowedAt = DateTime.UtcNow;
        }

        public int FollowerId { get; set; }
        public int FollowedId { get; set; }
        public DateTime FollowedAt { get; set; }
        public User Follower { get; set; }
        public User Followed { get; set; }
    }
}
