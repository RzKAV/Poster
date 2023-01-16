using Microsoft.Extensions.DependencyInjection;
using Poster.Logic.Services.Account;
using Poster.Logic.Services.Comments;
using Poster.Logic.Services.Posts;

namespace Poster.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddLogic(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPostsService, PostsService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}