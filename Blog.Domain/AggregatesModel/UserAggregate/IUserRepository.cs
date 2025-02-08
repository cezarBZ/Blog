using Blog.Domain.Core.Data;

namespace Blog.Domain.AggregatesModel.UserAggregate;

public interface IUserRepository : IRepository<User, int>
{
    Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
    Task<User> GetByIdWithFollowedAsync(int id);
    Task<IReadOnlyList<User>> GetFollowersAsync(int userId);
    Task<IReadOnlyList<User>> GetFollowedAsync(int userId);
}
