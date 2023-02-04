using Poster.Domain.Entities;

namespace Poster.Logic.Services.Tokens;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);

    Task<string> CreateRefreshToken(int userId);
}