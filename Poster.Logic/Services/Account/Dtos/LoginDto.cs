namespace Poster.Logic.Services.Account.Dtos;

public class LoginDto
{
    public bool ByRefreshToken { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string RefreshToken { get; set; }
}