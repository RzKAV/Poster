using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Poster.Domain.Entities;
using Poster.Logic.Common.AppConfig.Main;

namespace Poster.Logic.Services.Tokens;

internal class TokenService : ITokenService
{
    private readonly AuthOptions _authOptions;
    private readonly AppDbContext _dbContext;

    public TokenService(IOptions<AuthOptions> authOptions,
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<string> CreateRefreshToken(int userId)
    {
        if (userId < 1)
        {
            throw new ArgumentException(nameof(userId));
        }

        var token = await _dbContext.Tokens.FirstOrDefaultAsync(token =>
            token.UserId == userId
            && token.Client == _authOptions.Audience);

        var expireTime = DateTime.Now.AddDays(_authOptions.ExpireTimeRefreshTokenDays);

        var tokenValue = $"{userId}-{Guid.NewGuid():D}";

        if (token == null)
        {
            var newToken = new Token
            {
                Client = _authOptions.Audience,
                ExpireTime = expireTime,
                UserId = userId,
                Value = tokenValue
            };

            _dbContext.Tokens.Add(newToken);
            await _dbContext.SaveChangesAsync();
            return newToken.Value;
        }

        token.ExpireTime = expireTime;
        token.Value = tokenValue;
        token.UpdatedTime = DateTime.Now;
        await _dbContext.SaveChangesAsync();

        return token.Value;
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