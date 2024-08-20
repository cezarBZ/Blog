namespace Blog.Domain.AggregatesModel.PostAggregate;

public class Post : 
{
    public Post(string title, string content)
    {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        Updated = null;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime Created { get; private set; } = DateTime.Now;
    public DateTime? Updated { get; private set; }
    public List<Comment> Comments { get; private set; } = new List<Comment>();

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public void RemoveComment(Comment comment)
    {
        Comments.Remove(comment);
    }


}
