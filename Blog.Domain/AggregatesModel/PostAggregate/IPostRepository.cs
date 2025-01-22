using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.Core.Data;
namespace Blog.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository : IRepository<Post, int>
{
    Task AddLikeAsync(Like like);
    Task<Like> GetLikeByUserIdAndPostIdAsync(int userId, int postId);
}
