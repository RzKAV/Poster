namespace Poster.Logic.Services.Account;

public interface IAccountService
{
    public Task<List<UserDto>> GetUsers();

    public Task<string> Login(LoginDto loginDto);

    public Task<int> Register(RegisterDto registerDto);
}