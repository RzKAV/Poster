using Microsoft.EntityFrameworkCore;
using Poster.Logic;

namespace Poster.Infrastructure;

public static class AppDbContextSeed
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}