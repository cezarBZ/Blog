namespace Blog.Application.Responses
{
    public class PostLikesResponse
    {
        public int PostId { get; set; }
        public List<PostLikeViewModel> Likes { get; set; } = new List<PostLikeViewModel>();
        public int TotalLikes { get; set; }
    }
}
