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
            var username = HttpContext.Session.GetString(_configuration.GetSection("UsernameSessionKey").ToString());
                
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
                    
                    return View(questions);
                }
                
                return View();
            } 
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(string questionTitle, string questionDescription)
        {
            var userId = HttpContext.Session.GetString(_configuration.GetSection("UserIdSessionKey").ToString());

            var publicationDate = DateTime.Today;

            var newQuestion = new Question()
            {
                Title = questionTitle,
                Description = questionDescription,
                AuthorId = Convert.ToInt32(userId),
                PublicationDate = publicationDate
            };
            
            using (var client = new HttpClient())
            { 
                string actionName = $"AddQuestion";
                
                client.BaseAddress = new Uri(BaseAddress + actionName);

                HttpResponseMessage result = client.PostAsJsonAsync(actionName, newQuestion).Result;

            } 
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FindQuestions(string title)
        {
            using (var client = new HttpClient())
            { 
                string actionName = $"FindQuestions?title={title}";
                
                client.BaseAddress = new Uri(BaseAddress + actionName);

                HttpResponseMessage result = client.GetAsync(actionName).Result;
                
                if (result.IsSuccessStatusCode)
                {
                    var questions = await result.Content.ReadAsAsync<IEnumerable<PopularQuestionsViewModel>>();
                    
                    return View("Index", questions);
                }

            } 
            return View("Index", new List<PopularQuestionsViewModel>());
        }

        [HttpGet("Question")]
        public IActionResult QuestionPage(int id)
        {
            return View();
        }
    }
}