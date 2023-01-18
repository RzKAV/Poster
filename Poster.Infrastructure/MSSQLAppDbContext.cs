using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Poster.Logic;

namespace Poster.Infrastructure;

internal class MSSQLAppDbContext : AppDbContext
{
    public MSSQLAppDbContext(DbContextOptions<MSSQLAppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}