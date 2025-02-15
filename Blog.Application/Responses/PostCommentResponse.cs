namespace Blog.Application.Responses
{
    public class PostCommentResponse
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfilePicture { get; set; }
        public int LikeCount { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
