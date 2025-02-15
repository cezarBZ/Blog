namespace Blog.Application.Responses
{
    public class PostLikesResponse
    {
        public int PostId { get; set; }
        public List<PostLikeResponse> Likes { get; set; } = new List<PostLikeResponse>();
        public int TotalLikes { get; set; }
    }
}
