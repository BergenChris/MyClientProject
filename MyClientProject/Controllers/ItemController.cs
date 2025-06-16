using Microsoft.AspNetCore.Mvc;
using MyClientProject.Models;
using MyClientProject.Repos;
using MyClientProject.Filters;
using System.Text.Json;
using MyClientProject.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MyClientProject.Controllers
{
    [KlantSessionAuthorize]
    public class ItemController : Controller
    {
        private readonly IUserService _users;
        private User _user;

        private readonly IItemService _items;
        private readonly IOrderService _orders;

        public ItemController(IUserService _users, IItemService _items, IOrderService _orders)
        {
            this._users = _users;
            this._items = _items;
            this._orders = _orders;
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


        private async Task<User> GetUserAsync()
        {
            // Replace with your actual method of fetching user from database
            // e.g., based on logged-in user identity
            User();
            var userId = _user.UserId;
            if (userId == null)
                return null;

            return await _users.GetUserByIdAsync(userId);
        }


        [HttpGet]
        public async Task<IActionResult> Index(int itemId)
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

        [HttpGet]
        public async Task<IActionResult> Edit(int itemId)
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


        [HttpPost]
        public async Task<IActionResult> EditItem(Item updatedItem)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedItem);
            }

            bool updated = await _items.UpdateItemAsync(updatedItem.ItemId, updatedItem);
            if (!updated)
            {
                ModelState.AddModelError("", "Item niet gevonden.");
                return View(updatedItem);
            }

            return RedirectToAction("Index", "User");
        }

    }
}
