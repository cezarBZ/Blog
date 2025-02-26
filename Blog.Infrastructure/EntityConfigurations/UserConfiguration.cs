using Blog.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", Data.ApplicationDbContext.DEFAULT_SCHEMA);

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(u => u.Active)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.LastLoginAt)
                   .IsRequired(false);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasIndex(u => u.Username)
                   .IsUnique();

            builder.Property(p => p.FollowersCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.FollowingCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.ProfilePictureUrl)
                .HasColumnType("nvarchar(max)");

            builder.Property(l => l.Role)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),
                       v => (UserRole)Enum.Parse(typeof(UserRole), v)
                   )
                   .HasMaxLength(50);

            builder.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_User_Comments");

            builder.HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_Likes");

            builder.HasMany(u => u.Followers)
                   .WithOne(uf => uf.Followed)
                   .HasForeignKey(uf => uf.FollowedId);

            builder.HasMany(u => u.Following)
                   .WithOne(uf => uf.Follower)
                   .HasForeignKey(uf => uf.FollowerId);

        }
    }
}
