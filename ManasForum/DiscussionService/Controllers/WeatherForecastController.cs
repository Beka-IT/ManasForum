using DiscussionService.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace DiscussionService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly DiscussionServiceContext _context;

    public WeatherForecastController(DiscussionServiceContext context)
    {
        _context = context;
    }

    [HttpGet]
    public void ShowUsername()
    {
        Account user = _context.Accounts.FirstOrDefault();
        
        Console.WriteLine(user.Login);
    }
}