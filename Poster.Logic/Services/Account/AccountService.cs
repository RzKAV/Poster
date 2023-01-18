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

    public AccountService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        return await _userManager.Users.Select(user => new UserDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email
        }).ToListAsync();
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        if (!UserNameValidator.IsValidUserName(loginDto.UserName)) throw new CustomException();

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new CustomException();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var secretBytes = Encoding.UTF8.GetBytes("Super_mega_secret_key");

        var key = new SymmetricSecurityKey(secretBytes);

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "localhost:5001",
            "localhost:5001",
            claims,
            DateTime.Now,
            DateTime.Now.AddMinutes(30),
            signingCredentials);

        var value = new JwtSecurityTokenHandler().WriteToken(token);

        return value;
    }

    public async Task<int> Register(RegisterDto registerDto)
    {
        if (!(UserNameValidator.IsValidUserName(registerDto.UserName)
              && EmailValidator.IsValidEmail(registerDto.Email)))
            throw new CustomException();

        var user = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email.ToLower()
        };

        var createResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (!createResult.Succeeded) throw new CustomException();

        var roleResult = await _userManager.AddToRoleAsync(user, "user");

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            throw new CustomException();
        }

        return user.Id;
    }
}