using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyClientProject.Repos;
using MyClientProject.Filters;
using MyClientProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using System.Security.Claims;
using MyClientProject.Services.Interfaces;

namespace MyClientProject.Controllers
{
    [KlantSessionAuthorize]
    public class UserController : Controller
    {
        private readonly IUserService _users;
        private readonly IItemService _items;
        private User _user;


        public UserController(IUserService _users,IItemService _items)
        {
            this._users = _users;
            this._items = _items;
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
            User();
            if (_user != null)
            {
                ViewBag.Items = _items.GetAllItems();
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
