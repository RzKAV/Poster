using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Poster.Domain.Consts;
using Poster.Domain.Entities;
using Poster.Logic.Common.AppConfig.Main;
using Poster.Logic.Common.Exceptions.Api;
using Poster.Logic.Common.Validators;
using Poster.Logic.Services.Account.Dtos;
using Poster.Logic.Services.Tokens;
using Poster.Logic.Services.Tokens.Dtos;

namespace Poster.Logic.Services.Account;

internal class AccountService : IAccountService
{
    private readonly AuthOptions _authOptions;
    private readonly AppDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountService(UserManager<AppUser> userManager,
        ITokenService tokenService,
        AppDbContext dbContext,
        IOptions<AuthOptions> authOptions)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _dbContext = dbContext;
        _authOptions = authOptions.Value;
    }

    public async Task<TokenDto> Login(LoginDto loginDto)
    {
        if (loginDto.ByRefreshToken)
        {
            return await LoginByRefreshToken(loginDto);
        }

        return await LoginByPassword(loginDto);
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

    private async Task<TokenDto> LoginByPassword(LoginDto loginDto)
    {
        if (!UserNameValidator.IsValidUserName(loginDto.UserName))
        {
            throw new CustomException("InvalidUserName");
        }

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new CustomException("IncorrectUserNameOrPassword");
        }

        return new TokenDto
        {
            AccessToken = _tokenService.CreateAccessToken(user),
            RefreshToken = await _tokenService.CreateRefreshToken(user.Id)
        };
    }

    private async Task<TokenDto> LoginByRefreshToken(LoginDto loginDto)
    {
        var user = await GetUserByRefreshToken(loginDto.RefreshToken);

        var token = await _dbContext.Tokens.FirstOrDefaultAsync(token =>
            token.UserId == user.Id
            && token.Client == _authOptions.Audience
            && token.Value == loginDto.RefreshToken);

        if (token == null || token.ExpireTime < DateTime.Now)
        {
            throw new CustomException("InvalidRefreshToken");
        }

        return new TokenDto
        {
            AccessToken = _tokenService.CreateAccessToken(user),
            RefreshToken = await _tokenService.CreateRefreshToken(user.Id)
        };
    }

    private async Task<AppUser> GetUserByRefreshToken(string tokenValue)
    {
        var dividerIndex = tokenValue.IndexOf('-');

        if (dividerIndex < 0)
        {
            throw new CustomException("InvalidRefreshToken");
        }

        var id = tokenValue[..dividerIndex];

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            throw new CustomException("InvalidLogin");
        }

        return user;
    }
}