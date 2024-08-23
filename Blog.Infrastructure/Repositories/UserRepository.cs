using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories;

public class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

}
