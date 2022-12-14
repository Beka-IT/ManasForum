using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DiscussionService.Models;
using DiscussionService.ViewModels;
using Grpc.Net.Client;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private const string BaseAddress = "https://localhost:7176/Question/";
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString(_configuration.GetSection("UserSessionKey").ToString());
                
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            using (var client = new HttpClient())
            { 
                string actionName = $"PopularQuestions";
                
                client.BaseAddress = new Uri(BaseAddress + actionName);

                //HTTP POST
                HttpResponseMessage result = client.GetAsync(actionName).Result;

                if (result.IsSuccessStatusCode)
                {
                    var questions = await result.Content.ReadAsAsync<IEnumerable<PopularQuestionsViewModel>>();
                    Console.WriteLine(questions.Count());
                    return View(questions);
                }
                
                return View();
            } 
        }
    }
}