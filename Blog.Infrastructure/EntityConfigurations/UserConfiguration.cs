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
        }
    }
}
