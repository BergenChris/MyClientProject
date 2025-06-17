using Microsoft.AspNetCore.Mvc;
using MyClientProject.Services.Interfaces;
using MyClientProject.Filters;
using MyClientProject.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MyClientProject.Services;

namespace MyClientProject.Controllers
{
    [KlantSessionAuthorize]
    public class ShopController:Controller
    {

        private readonly IUserService _users;
        private User _user;

        private readonly IItemService _items;
      

        public ShopController(IUserService _users, IItemService _items)
        {
            this._users = _users;
            this._items = _items;
         
        }


        // function for guests since they are not in DB but in session
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

        // gets userdata from session and then compares it with users in DB
        private async Task<User> GetUserAsync()
        {
            User();
            var userId = _user.UserId;
            if (userId == null)
                return null;

            return await _users.GetUserByIdAsync(userId);
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
                return Redirect("/");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Item(int itemId)
        {

            Item item = await _items.GetAsync(itemId);
            if (item == null)
            {
                Console.WriteLine($"Item with ID {itemId} was NOT found.");
                return NotFound(); // Optional but good to add
            }

            Console.WriteLine($"Item found: {item.Name}, Price: {item.Price}");

            return View(item);

        }













    }
}
