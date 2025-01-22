using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
    }
}
