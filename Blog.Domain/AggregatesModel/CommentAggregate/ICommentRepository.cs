using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.CommentAggregate
{
    public interface ICommentRepository: IRepository<Comment, int>
    {
    }
}
