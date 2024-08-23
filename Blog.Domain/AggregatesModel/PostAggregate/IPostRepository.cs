using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository : IRepository<Post, int>
{ }
