using Blog.Domain.AggregatesModel.PostAggregate;

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
        public List<CommentsResponse> Comments { get; set; }

    }
}
