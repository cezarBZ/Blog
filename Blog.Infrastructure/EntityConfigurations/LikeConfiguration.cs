using Blog.Domain.AggregatesModel.LikeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.EntityConfigurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Like", Data.ApplicationDbContext.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Post)
            .WithMany(c => c.Likes)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Like_Post");

            builder.HasOne(p => p.User)
            .WithMany(c => c.Likes)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Like_User");

            builder.Property(p => p.LikedAt)
                .IsRequired()
                .HasColumnType("datetime");
        }
    }
}
