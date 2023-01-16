using Microsoft.AspNetCore.Identity;

namespace Poster.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public List<Post>? Posts { get; set; }

    public List<Comment>? Comments { get; set; }
}