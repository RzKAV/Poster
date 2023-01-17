using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poster.Domain.Entities;

namespace Poster.Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(z => z.Text).IsRequired();
        
        builder
            .HasOne(z => z.Post)
            .WithMany(x => x.Comments)
            .HasForeignKey(z => z.PostId);
    }
}