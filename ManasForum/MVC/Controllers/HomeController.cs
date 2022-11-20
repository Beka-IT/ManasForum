using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            
        }

        public async Task<IActionResult> Index()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}