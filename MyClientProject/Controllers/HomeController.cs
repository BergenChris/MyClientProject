using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyClientProject.Models;
using MyClientProject.Repos;
using System.Diagnostics;
using System.Text.Json;
using MyClientProject.Services.Interfaces;

namespace MyClientProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _users;

        public HomeController(IUserService _users)
        {
            this._users=_users;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inloggen(User user)
        {
          
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.UserEmail))
            {
          
                return View("Index"); 
            }

            var gevondenKlant = _users
            .GetAll()
            .FirstOrDefault(k => k.Name.ToLower() == user.Name.ToLower()  && k.UserEmail.ToLower() == user.UserEmail.ToLower());

           
            if (gevondenKlant != null )
            {
                // Zet klant in session als JSON string
                var klantJson = JsonSerializer.Serialize(gevondenKlant);
                HttpContext.Session.SetString("User", klantJson);
               
                return RedirectToAction("Index", "User");
            }



            //Guest-route
            else 
            {
                // Zet klant in session als JSON string
                var klantJson = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString("User", klantJson);
              
                

                return RedirectToAction("Index", "User");
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
