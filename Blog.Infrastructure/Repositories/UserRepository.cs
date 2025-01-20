using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class UserRepository : Repository<User, int>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
    }

    public void UpdateLastLogin(User user)
    {
        user.RegisterLogin();
    }
}
