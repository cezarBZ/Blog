﻿using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.Core.Models;
using System.Data;

namespace Blog.Domain.AggregatesModel.UserAggregate;

public class User : Entity<int>, IAggregateRoot
{
    public User(string username, string email, string passwordHash, bool active, string role)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        Active = active;
        Role = role;
    }

    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool Active { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();



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
}