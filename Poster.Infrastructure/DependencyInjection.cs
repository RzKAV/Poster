using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poster.Logic;

namespace Poster.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MSSQLAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

        services.AddScoped<AppDbContext>(provider => provider.GetService<MSSQLAppDbContext>()!);

        return services;
    }
}