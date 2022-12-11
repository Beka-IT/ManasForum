using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}