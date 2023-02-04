using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poster.Logic.Common.AppConfig.Configurations;
using Poster.Logic.Services.Account;
using Poster.Logic.Services.Comments;
using Poster.Logic.Services.Posts;
using Poster.Logic.Services.Tokens;

namespace Poster.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddLogic(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAppSettingHelper(configuration);

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPostsService, PostsService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddTransient<ITokenService, TokenService>();

        return services;
    }
}