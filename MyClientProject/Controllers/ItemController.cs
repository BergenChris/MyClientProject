using Microsoft.AspNetCore.Mvc;
using MyClientProject.Models;
using MyClientProject.Repos;
using MyClientProject.Filters;
using System.Text.Json;

namespace MyClientProject.Controllers
{
    [KlantSessionAuthorize]
    public class ItemController : Controller
    {
        private readonly UserRepo _userRepo;
        private User _user;

        private readonly ItemRepo _itemRepo;

        public ItemController(UserRepo _userRepo, ItemRepo _itemRepo)
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
        public IActionResult Index(int id)
        {
            User();
            if (_user != null)
            {
                ViewBag.Item = _itemRepo.Get(id);
                return View("item",_user);
            }
            else
            {
                return Redirect("/");
            }

        }
    }
}
