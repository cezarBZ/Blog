namespace Blog.Domain.AggregatesModel.PostAggregate;

public class Comment
{
    public Comment(string content, Guid postId)
    {
        Id = Guid.NewGuid();
        Content = content;
        CreatedAt = DateTime.Now;
        UpdatedAt = null;
        PostId = postId;
    }

    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void EditContent(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new ArgumentException("O conteúdo do comentário não pode ser vazio.", nameof(newContent));
        }

        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }

}
