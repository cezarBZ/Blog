using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.CommentAggregate
{
    public interface ICommentRepository: IRepository<Comment, int>
    {
        Task<IReadOnlyList<Comment>> GetPostComments(int postId);

    }
}
