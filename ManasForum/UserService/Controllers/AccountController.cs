using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string login, string password)
    {
        if (login == null || password == null)
        {
            return BadRequest("Введите логин и пароль!");
        }
        
        var user = await _accountService.LoginAsync(login, password);

        if (user == null)
        {
            return Unauthorized("Неправильный логин или пароль!");
        }

        return Ok();
    }

    [HttpPost("SignUp")]
    public async Task<AccountSignUpResponse> SignUp(Account account)
    {
        var userResponse = await _accountService.SignUpAsync(account);

        return userResponse;
    }
}