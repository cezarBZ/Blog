public class GetPostLikesViewModel
{
    public int PostId { get; set; }
    public List<PostLikeDto> Likes { get; set; } = new List<PostLikeDto>();
    public int TotalLikes { get; set; }
}