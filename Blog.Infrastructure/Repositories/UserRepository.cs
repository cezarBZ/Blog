﻿using Blog.Domain.AggregatesModel.UserAggregate;
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

    public async Task<User> GetByIdWithFollowedAsync(int id)
    {
        return await _dbContext.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IReadOnlyList<User>> GetFollowersAsync(int userId)
    {
        return await _dbContext.Users
            .Include(u => u.Followers)
            .Where(u => u.Following.Any(f => f.FollowedId == userId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<User>> GetFollowedAsync(int userId)
    {
        return await _dbContext.Users
            .Include(u => u.Following)
            .Where(u => u.Followers.Any(f => f.FollowerId == userId))
            .ToListAsync();
    }

}
