using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{

    public AccountController()
    {
    }

    [HttpPost]
    public string Login(string login, string password)
    {
        if (login == "Beka" && password == "aka2")
        {
            return "Добро пожаловать!";
        }

        return "Неправильный пароль или логин!";
    }
}