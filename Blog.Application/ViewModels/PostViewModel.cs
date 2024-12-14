namespace Blog.Application.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel(string title, string content, string coverImageUrl, DateTime createdAt, DateTime? updatedAt)
        {
            Title = title;
            Content = content;
            CoverImageUrl = coverImageUrl;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CoverImageUrl { get; private set; }
    }
}
