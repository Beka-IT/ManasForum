using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString(_configuration.GetSection("UserSessionKey").ToString());
                
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            
            return View();
        }

    }
}