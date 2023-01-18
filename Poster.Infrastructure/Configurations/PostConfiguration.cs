using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poster.Domain.Entities;

namespace Poster.Infrastructure.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(post => post.Text).IsRequired();

        builder
            .HasOne(post => post.User)
            .WithMany(user => user.Posts)
            .HasForeignKey(post => post.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}