using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Poster.Domain.Entities;
using Poster.Logic.Common.Exceptions.Api;
using Poster.Logic.Common.Validators;
using Poster.Logic.Services.Account.Dtos;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Poster.Logic.Services.Account;

internal class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;

    private readonly IAppDbContext _dbContext;

    public AccountService(UserManager<AppUser> userManager, IAppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        return await _userManager.Users.Select(user => new UserDto
        {
            UserId = user.Id,
            UserName = user.UserName
        }).ToListAsync();
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        if (!UserNameValidator.IsValidUserName(loginDto.UserName))
        {
            throw new CustomException();
        }
        
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new CustomException();
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var secretBytes = Encoding.UTF8.GetBytes("Super_mega_secret_key");

        var key = new SymmetricSecurityKey(secretBytes);

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "localhost:5001",
            audience: "localhost:5001",
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signingCredentials);

        var value = new JwtSecurityTokenHandler().WriteToken(token);

        return value;
    }

    public async Task<int> Register(RegisterDto registerDto)
    {
        if (!UserNameValidator.IsValidUserName(registerDto.UserName))
        {
            throw new CustomException();
        }
        
        if (!EmailValidator.IsValidEmail(registerDto.Email))
        {
            throw new CustomException();
        }

        
        var user = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email.ToLower()
        };

        var createResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (!createResult.Succeeded)
        {
            return -1;
        }
        
        var roleResult = await _userManager.AddToRoleAsync(user, "user");
        
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return -2;
        }

        return user.Id;
    }
}