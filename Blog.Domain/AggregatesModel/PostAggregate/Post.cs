using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.PostAggregate;

public class Post : Entity<int>, IAggregateRoot
{
    public Post(string title, string content, string coverImageUrl)
    {
        Title = title;
        Content = content;
        CoverImageUrl = coverImageUrl;
        UpdatedAt = null;
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; private set; }
    public string CoverImageUrl { get; private set; }
    public void Edit(string newTitle, string newContent, string newCoverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("Post title can not be empty.", nameof(newTitle));
        }

        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new ArgumentException("Post content can not be empty.", nameof(newContent));
        }

        Content = newContent;
        Title = newTitle;
        CoverImageUrl = newCoverImageUrl ?? CoverImageUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}
