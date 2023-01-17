using Microsoft.AspNetCore.Mvc;
using Poster.Logic.Services.Account;
using Poster.Logic.Services.Account.Dtos;

namespace Poster.Controllers;

[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _accountService.GetUsers();
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _accountService.Register(registerDto);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _accountService.Login(loginDto);
        return Ok(result);
    }
}