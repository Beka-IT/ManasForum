using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DiscussionService.Migrations;
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
                    var questions = await result.Content.ReadAsAsync<IEnumerable<QuestionsViewModel>>();
                    
                    return View(questions);
                }
                
                return View();
            } 
        }

        [HttpGet]
        public async Task<IActionResult> Members()
        {
            return View();
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
                    var questions = await result.Content.ReadAsAsync<IEnumerable<QuestionsViewModel>>();
                    
                    return View("Index", questions);
                }

            } 
            
            return View("Index", new List<QuestionsViewModel>());
        }

        [HttpGet]
        [Route("Question/{id:int}")]
        public async Task<IActionResult> Question(int id)
        {
            using (var client = new HttpClient())
            { 
                string userId = HttpContext.Session.GetString(_configuration.GetSection("UserIdSessionKey").ToString());
                string actionName = $"GetQuestion?id={id}&userId={userId}";
                
                client.BaseAddress = new Uri(BaseAddress + actionName);

                HttpResponseMessage result = client.GetAsync(actionName).Result;
                
                if (result.IsSuccessStatusCode)
                {
                    var question = await result.Content.ReadAsAsync<QuestionPageViewModel>();
                    
                    return View("Question", question);
                }

            } 
            
            return View(new QuestionPageViewModel());
        }
        
        [HttpPost("AddAnswer")]
        public IActionResult AddAnswer(int questionId, string description)
        {
            string userId = HttpContext.Session.GetString(_configuration.GetSection("UserIdSessionKey").ToString());
            
            var answer = new Answer()
            {
                AuthorId = Convert.ToInt32(userId),
                QuestionId = questionId,
                Description = description
            };
            
            using (var client = new HttpClient())
            { 
                string actionName = $"Answer";
                
                client.BaseAddress = new Uri(BaseAddress + actionName);

                HttpResponseMessage result = client.PostAsJsonAsync(actionName, answer).Result;
            }

            return Redirect($"/Question/{questionId}");
        }
    }
}