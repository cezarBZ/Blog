using Blog.Domain.AggregatesModel.PostAggregate;
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

            builder.Property(p => p.CoverImageUrl)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(p => p.User)
            .WithMany(c => c.Posts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Post_User");

        }
    }
}
