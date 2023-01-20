using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Poster.Domain.Entities;
using Poster.Logic.Common.AppConfig.Main;

namespace Poster.Logic.Services.Token;

internal class TokenService : ITokenService
{
    private readonly AuthOptions _authOptions;

    public TokenService(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    public string CreateAccessToken(AppUser user)
    {
        if (user == null)
        {
            throw new ArgumentException(nameof(user));
        }

        var token = CreateSecurityToken(user);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken CreateSecurityToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        return new JwtSecurityToken(
            _authOptions.Issuer,
            _authOptions.Audience,
            claims,
            DateTime.Now,
            DateTime.Now.AddMinutes(_authOptions.ExpireTimeTokenMinutes),
            new SigningCredentials(_authOptions.SymmetricSecurityKey,
                SecurityAlgorithms.HmacSha256));
    }
}