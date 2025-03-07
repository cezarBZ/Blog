using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.UserAggregate;

public class User : Entity<int>, IAggregateRoot
{
    public User(string username, string email, string passwordHash, bool active, UserRole role)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        Active = active;
        Role = role;
        FollowersCount = 0;
        FollowingCount = 0;
    }
    // Novo construtor para testes
    public User(int id, string username, string email, string passwordHash, bool active, UserRole role)
        : this(username, email, passwordHash, active, role)
    {
        Id = id; // Define o Id diretamente
    }

    public string Username { get; set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool Active { get; set; }
    public UserRole Role { get; set; }
    public int FollowersCount { get; private set; }
    public int FollowingCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public string ProfilePictureUrl { get; private set; }

    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UserFollower> Followers { get; set; } = new List<UserFollower>();
    public ICollection<UserFollower> Following { get; set; } = new List<UserFollower>();



    public void Update(string fullName, string email, string password)
    {
        Username = fullName;
        Email = email;
        PasswordHash = password;
    }

    public void Inactive(bool active)
    {
        Active = active;
    }

    public void RegisterLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Follow(User userToFollow)
    {
        if (userToFollow.Id == Id)
            throw new InvalidOperationException("A user cannot follow themselves.");

        if (Following.Any(uf => uf.FollowedId == userToFollow.Id))
            throw new InvalidOperationException("The user is already being followed.");

        var userFollower = new UserFollower(Id, userToFollow.Id);
        Following.Add(userFollower);
        FollowingCount++;
        userToFollow.Followers.Add(userFollower);
        userToFollow.IncrementFollowersCount();
    }

    public void Unfollow(User userToUnfollow)
    {
        if (userToUnfollow.Id == Id)
            throw new InvalidOperationException("A user cannot unfollow themselves.");

        var userFollower = Following.SingleOrDefault(uf => uf.FollowedId == userToUnfollow.Id);
        if (userFollower == null)
            throw new InvalidOperationException("The user is not being followed.");

        Following.Remove(userFollower);
        FollowingCount--;
        userToUnfollow.Followers.Remove(userFollower);
        userToUnfollow.DecrementFollowersCount();
    }

    public void IncrementFollowersCount()
    {
        FollowersCount++;
    }

    public void DecrementFollowersCount()
    {
        if (FollowersCount > 0)
            FollowersCount--;
    }
}