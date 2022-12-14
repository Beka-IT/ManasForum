using System.Net.Http.Headers;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using UserService.Models;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace MVC.Controllers
{
    public class UserController : Controller
    {

        private const string _baseAddress = "https://localhost:7197/Account/";

        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
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

        [HttpPost("SignUpPost")]
        public async Task<IActionResult> SignUpPost(AccountSignUpDto newAccount)
        {
            using (var client = new HttpClient())
            { 
                string actionName = "SignUp";
                
                client.BaseAddress = new Uri(_baseAddress + actionName);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(actionName, newAccount).Result;
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    var accountSignUpResponse = await response.Content.ReadAsAsync<AccountSignUpResponse>();

                    if (accountSignUpResponse.Account != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                    TempData["SignUpError"] = accountSignUpResponse.message;
                }
                
                return RedirectToAction("SignUp");
            }
        }

        [HttpPost("LoginPost")]
        public async Task<ActionResult> LoginPost(AccountDto account)
        {
            if (account.Login == null || account.Password == null)
            {
                TempData["LoginError"] = "Введите логин и пароль!";
                
                return RedirectToAction("Login");
            }
            
            using (var client = new HttpClient())
            { 
                string actionName = "Login";
                
                client.BaseAddress = new Uri(_baseAddress + actionName);

                //HTTP POST
                HttpResponseMessage result = client.PostAsJsonAsync(actionName, account).Result;

                if (result.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString(_configuration.GetSection("UserSessionKey").ToString(), account.Login);
                    return RedirectToAction("Index", "Home");
                }
                
                TempData["LoginError"] = "Неправильный логин или пароль!";
                
                return RedirectToAction("Login");
            }
        }

        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove(_configuration.GetSection("UserSessionKey").ToString());

            return RedirectToAction("Login", "User");
        }
    }
}