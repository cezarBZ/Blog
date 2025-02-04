﻿using Blog.Domain.AggregatesModel.CommentAggregate;
using Blog.Domain.AggregatesModel.LikeAggregate;
using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Blog.Domain.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;


namespace Blog.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "BLOG";
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserFollower> UserFollowers { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
public class ApplicationDataContextDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer();

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}