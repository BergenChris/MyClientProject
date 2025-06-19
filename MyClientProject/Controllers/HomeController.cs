using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyClientProject.Models;
using MyClientProject.Repos;
using System.Diagnostics;
using System.Text.Json;
using MyClientProject.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using MyClientProject.Services;

namespace MyClientProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _users;
        private User _user;
        private readonly ISeedService _seedService;

        public HomeController(IUserService _users, ISeedService seedService)
        {
            this._users=_users;
            this._seedService = seedService;
        }

        private async Task<User> User()
        {
            var klantJson = HttpContext.Session.GetString("User");
  
            if (!string.IsNullOrEmpty(klantJson))
            {
                _user = JsonSerializer.Deserialize<User>(klantJson);
                if (_user.ShoppingList.IsNullOrEmpty())
                {
                    _user.ShoppingList = new List<int>();
                }
            }
            return _user;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inloggen(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.UserEmail))
            {
                ModelState.AddModelError("", "Naam en e-mailadres zijn verplicht.");
                return View("Index");
            }

            // Try to find user in the database
            var gevondenKlant = _users
                .GetAll()
                .FirstOrDefault(k =>
                    k.Name.ToLower() == user.Name.ToLower() &&
                    k.UserEmail.ToLower() == user.UserEmail.ToLower());

            if (gevondenKlant != null)
            {
                // Found: store user in session
                var klantJson = JsonSerializer.Serialize(gevondenKlant);
                HttpContext.Session.SetString("User", klantJson);
            }
            else
            {
                Guest();
            }

            return RedirectToAction("Index", "Shop");
        }
        [HttpGet]
        public IActionResult Guest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateGuest(User user)
        {
            if (ModelState.IsValid)
            {
                var klantJson = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString("User", klantJson);
                return RedirectToAction("Index","Shop");
            }
            else
            {
                return View("Guest");
            }
        }


        [HttpGet]
        public IActionResult Account()
        {
            return View("CreateAccount");
        
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(User user)
        {
            if (ModelState.IsValid)
            {
                await _users.CreateUserAsync(user);  // await here!

                return RedirectToAction("Inloggen", user);
            }
            else
            {
                return View("CreateAccount", user);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to the home page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SeedDatabase()
        {
            await _seedService.SeedDatabaseAsync();
            return RedirectToAction("Index");
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
