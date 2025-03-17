using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;

namespace Blog.Application.Responses
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CoverImageUrl { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public List<CommentsResponse> Comments { get; set; }
        public UserResponse User { get; set; }

    }
}
