using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Domain.AggregatesModel.UserAggregate;

namespace Blog.Infrastructure.EntityConfigurations
{
    public class UserFollowerConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder.HasKey(uf => new { uf.FollowerId, uf.FollowedId });

            builder.HasOne(uf => uf.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(uf => uf.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(uf => uf.Followed)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(uf => uf.FollowedId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(uf => uf.FollowedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()"); 
        }
    }
}