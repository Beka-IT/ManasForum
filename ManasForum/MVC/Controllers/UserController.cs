using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserService.Models;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        string Baseurl = "https://localhost:7197";
        public UserController()
        {
            
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpGet("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }
        /*
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var account = new Account();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.PostAsJsonAsync<Account>(account,"/Account/Login");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                }
                //returning the employee list to view
                return View(EmpInfo);
                
            }
        }*/
        
    }
}