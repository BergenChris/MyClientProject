using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyClientProject.Repos;
using MyClientProject.Filters;
using MyClientProject.Models;

namespace MyClientProject.Controllers
{
    [KlantSessionAuthorize]
    public class UserController : Controller
    {
        private readonly UserRepo _userRepo;
        private User _user;

        private readonly ItemRepo _itemRepo;

        public UserController(UserRepo _userRepo,ItemRepo _itemRepo)
        {
            this._userRepo = _userRepo;
            this._itemRepo = _itemRepo;
        }
        private void User()
        {
            var klantJson = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(klantJson))
            {
                _user = JsonSerializer.Deserialize<User>(klantJson);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            User();
            if (_user != null)
            {
                ViewBag.Items = _itemRepo.GetAll();
                if (_user.Role == "Client")
                {

                    return View("Client", _user);
                }
                else if (_user.Role == "Employer")
                {
                    return View("Employer", _user);
                }
                else 
                {
                    return View("Guest", _user);
                } 
            }
            else
            {
                return Redirect("/");            }

        }



    }
}
