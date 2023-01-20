using Poster.Domain.Entities;

namespace Poster.Logic.Services.Token;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);
}