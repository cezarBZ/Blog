using Blog.Domain.AggregatesModel.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.EntityConfigurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comment", Data.ApplicationDbContext.DEFAULT_SCHEMA);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Content)
            .IsRequired()
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.HasOne(c => c.Post)
                   .WithMany(u => u.Comments) 
                   .HasForeignKey(c => c.PostId)
                   .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(c => c.User)
               .WithMany(u => u.Comments) 
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.LikeCount)
                .IsRequired()
                .HasDefaultValue(0);

    }
}
