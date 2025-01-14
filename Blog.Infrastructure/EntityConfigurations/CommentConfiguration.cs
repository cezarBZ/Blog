

using Blog.Domain.AggregatesModel.PostAggregate;
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
            .HasColumnType("nvarchar(1000)")
            .HasMaxLength(50);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.HasOne(p => p.Post)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Comment_Post");

    }
}
