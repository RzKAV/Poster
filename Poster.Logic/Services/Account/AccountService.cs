using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Poster.Domain.Consts;
using Poster.Domain.Entities;
using Poster.Logic.Common.Exceptions.Api;
using Poster.Logic.Common.Validators;
using Poster.Logic.Services.Account.Dtos;
using Poster.Logic.Services.Token;

namespace Poster.Logic.Services.Account;

internal class AccountService : IAccountService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountService(UserManager<AppUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
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
        if (!UserNameValidator.IsValidUserName(loginDto.UserName))
        {
            throw new CustomException("invalid userName");
        }

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new CustomException("incorrect user or password");
        }

        return _tokenService.CreateAccessToken(user);
    }

    public async Task<int> Register(RegisterDto registerDto)
    {
        if (!(UserNameValidator.IsValidUserName(registerDto.UserName)
              && EmailValidator.IsValidEmail(registerDto.Email)))
        {
            throw new CustomException("invalid username or email");
        }

        var user = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email.ToLower()
        };

        var createResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (!createResult.Succeeded)
        {
            throw new CustomException(createResult.Errors.Select(e => e.Description).ToArray());
        }

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            throw new Exception("Error adding role");
        }

        return user.Id;
    }
}