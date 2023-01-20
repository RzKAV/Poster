using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poster.Logic.Common.AppConfig.Main;

namespace Poster.Logic.Common.AppConfig.Configurations;

public static class AppSettingsHelperExtension
{
    public static IServiceCollection AddAppSettingHelper(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AuthOptions>(
            configuration.GetSection("AuthOptions"));

        return services;
    }
}