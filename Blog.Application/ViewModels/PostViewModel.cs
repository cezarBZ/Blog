namespace Blog.Application.ViewModels
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
