using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poster.Domain.Entities;

namespace Poster.Infrastructure.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(z => z.Text).IsRequired();
        
        builder
            .HasOne(z => z.User)
            .WithMany(x => x.Posts)
            .HasForeignKey(z => z.UserId);
    }
}