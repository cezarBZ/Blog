﻿using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModel.PostAggregate;

public class Post : Entity<int>, IAggregateRoot
{
    public Post(string title, string content, string coverImageUrl, int createdBy)
    {
        Title = title;
        Content = content;
        CoverImageUrl = coverImageUrl;
        UpdatedAt = null;
        Comments = new List<Comment>();
        CreatedBy = createdBy;
        LikeCount = 0;
    }

    public Post(int id, string title, string content, string coverImageUrl, int createdBy)
    {
        Id = id;
        Title = title;
        Content = content;
        CoverImageUrl = coverImageUrl;
        UpdatedAt = null;
        Comments = new List<Comment>();
        CreatedBy = createdBy;
        LikeCount = 0;
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; private set; }
    public string CoverImageUrl { get; private set; }
    public List<Comment> Comments { get; private set; }
    public int LikeCount { get; private set; }
    public int CommentCount { get; private set; }

    public int CreatedBy { get; private set; }
    public User User { get;  set; }
    public void Edit(string newTitle, string newContent, string newCoverImageUrl)
    {
        Content = newContent;
        Title = newTitle;
        CoverImageUrl = newCoverImageUrl ?? CoverImageUrl;
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

    public void IncrementCommentCount()
    {
        CommentCount++;
    }

    public void DecrementCommentCount()
    {
        if (CommentCount > 0)
            CommentCount--;
    }

}
