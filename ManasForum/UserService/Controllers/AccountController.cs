using Microsoft.AspNetCore.Mvc;
using MVC.Models;
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
    public async Task<IActionResult> Login(AccountDto accountDto)
    {
        var user = await _accountService.LoginAsync(accountDto.Login, accountDto.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpPost("SignUp")]
    public async Task<AccountSignUpResponse> SignUp(AccountSignUpDto account)
    {
        AccountSignUpResponse userResponse = await _accountService.SignUpAsync(account);

        return userResponse;
    }
}