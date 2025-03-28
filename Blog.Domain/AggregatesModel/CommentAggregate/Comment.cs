﻿using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.CommentAggregate;

public class Comment : Entity<int>, IAggregateRoot
{
    public Comment(string content, int postId, int userId)
    {
        Content = content;
        CreatedAt = DateTime.Now;
        UpdatedAt = null;
        PostId = postId;
        UserId = userId;
    }

    public int PostId { get; private set; }
    public Post Post { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public int LikeCount { get; private set; }

    public void Edit(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new ArgumentException("O conteúdo do comentário não pode ser vazio.", nameof(newContent));
        }

        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementLikeCount()
    {
        LikeCount++;
    }

    public void DecrementLikeCount()
    {
        if (LikeCount > 0)
            LikeCount--;
    }
}
