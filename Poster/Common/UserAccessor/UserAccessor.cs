using System.Security.Claims;
using Poster.Logic.Common.UserAccessor;

namespace Poster.Common.UserAccessor;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserAccessor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentException();
    }

    public ClaimsPrincipal User => _contextAccessor.HttpContext.User;
    public int UserId => int.Parse(User.Identity.Name);
    public string Host => 
        @$"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host.Value}";
}