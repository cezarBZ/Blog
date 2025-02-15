namespace Blog.Application.Responses
{
    public class PostCommentsResponse
    {
        public int PostId { get; set; }
        public List<PostCommentResponse> Comments { get; set; } = new List<PostCommentResponse>();
        public int TotalComments { get; set; }
    }
}
