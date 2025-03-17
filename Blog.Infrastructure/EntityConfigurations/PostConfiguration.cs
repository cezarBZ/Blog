using Blog.Domain.AggregatesModel.PostAggregate;
using Blog.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post", Data.ApplicationDbContext.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnType("nvarchar(150)");

            builder.Property(p => p.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.UpdatedAt)
                .HasColumnType("datetime")
                .IsRequired(false);

            builder.HasOne(a => a.User)
               .WithMany()
               .IsRequired()
               .HasForeignKey(_ => _.CreatedBy)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Post_User");

            builder.Property(p => p.CoverImageUrl)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.LikeCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.CommentCount)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
