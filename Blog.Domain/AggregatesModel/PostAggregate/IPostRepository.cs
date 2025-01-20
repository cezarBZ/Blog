using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository : IRepository<Post, int>
{
    Task AddCommentAsync(Comment comment);
    Task AddLikeAsync(Like like);

    Task<Like> GetLikeByUserIdAndPostIdAsync(int userId, int postId);
}
