using Microsoft.EntityFrameworkCore;
using Poster.Domain.Entities;

namespace Poster.Logic;

public interface IAppDbContext
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}