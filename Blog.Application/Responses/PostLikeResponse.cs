public class PostLikeResponse
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserProfilePicture { get; set; }
    public DateTime LikedAt { get; set; }
}