using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poster.Domain.Entities;

namespace Poster.Infrastructure.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(user => user.UserName).IsRequired();
        
        builder
            .HasMany(user => user.Posts)
            .WithOne(post => post.User)
            .HasForeignKey(post => post.UserId);
    }
}