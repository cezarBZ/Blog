using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.PostAggregate;

public class Comment : Entity<int>
{
    public Comment(string content, int postId)
    {
        Content = content;
        CreatedAt = DateTime.Now;
        UpdatedAt = null;
        PostId = postId;
    }

    public int PostId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void Edit(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new ArgumentException("O conteúdo do comentário não pode ser vazio.", nameof(newContent));
        }

        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }

}
