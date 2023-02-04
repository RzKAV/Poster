using Poster.Logic.Services.Account.Dtos;
using Poster.Logic.Services.Tokens.Dtos;

namespace Poster.Logic.Services.Account;

public interface IAccountService
{
    public Task<TokenDto> Login(LoginDto loginDto);

    public Task<int> Register(RegisterDto registerDto);
}