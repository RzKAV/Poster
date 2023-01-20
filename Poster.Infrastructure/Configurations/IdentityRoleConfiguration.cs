using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poster.Domain.Consts;

namespace Poster.Infrastructure.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        builder.HasData(new IdentityRole<int>
        {
            Id = 1,
            Name = Roles.User,
            NormalizedName = Roles.User.ToUpper()
        }, new IdentityRole<int>
        {
            Id = 2,
            Name = Roles.Admin,
            NormalizedName = Roles.Admin.ToUpper()
        });
    }
}