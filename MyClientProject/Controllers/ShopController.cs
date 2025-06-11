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
        private readonly IOrderService _orders;

        public ShopController(IUserService _users, IItemService _items, IOrderService _orders)
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
        public async Task<IActionResult> Index()
        {
            User();
            
            var shoppingList = await _users.GetShoppingListFromUser(_user.UserId);
            decimal Prijs = 0;
            foreach (var x in shoppingList)
            {
                var item = await _items.GetAsync(x.ItemId);
                decimal discountedPrice = item.Price * (1 - ((decimal)_user.Discount / 100));
                Prijs += discountedPrice;
            }
            ViewBag.User = _user;
            ViewBag.Price = Prijs;

            return View(shoppingList);
        
        }
        [HttpPost]
        public async Task<IActionResult> AddToShoppingList(int itemId)
        {
            // Load the current user from the database
            var user = await GetUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            // Initialize shopping list if null
            if (user.ShoppingList == null || !user.ShoppingList.Any())
            {
                user.ShoppingList = new List<int>();
            }

            var item = await _items.GetAsync(itemId); // Assuming GetAsync is async
            if (item == null)
            {
                return NotFound();
            }

            if (item.StockQuantity > 0)
            {
                // Add item ID to shopping list
                user.ShoppingList.Add(item.ItemId);
                // Decrement stock quantity
                item.StockQuantity--;
                // Update item in repository
                await _items.UpdateItemAsync(item.ItemId);
                await _users.UpdateUserAsync(user);
                // Save changes to user
                await _users.SaveChangesAsync();

            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Index","User");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            // Initialize shopping list if null
            if (user.ShoppingList == null || !user.ShoppingList.Any())
            {
                user.ShoppingList = new List<int>();
            }

            var item = await _items.GetAsync(itemId); // Assuming GetAsync is async
            if (item == null)
            {
                return NotFound();
            }
            // Add item ID to shopping list
            user.ShoppingList.Remove(item.ItemId);
            // Decrement stock quantity
            item.StockQuantity++;
            // Update item in repository
            await _items.UpdateItemAsync(item.ItemId);
            await _users.UpdateUserAsync(user);
            // Save changes to user
            await _users.SaveChangesAsync();


            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Order(Order order)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            Task<List<int>> shoppingListIds = _users.GetShoppingListIdsFromUser(user.UserId);
            List<int> ids = await shoppingListIds;
            Task<List<Item>> shoppingList = _users.GetShoppingListFromUser(user.UserId);
            List<Item> items = await shoppingList;
            decimal Prijs = 0;
            foreach (var item in items)
            {
                decimal discountedPrice = item.Price * (1 - ((decimal)_user.Discount / 100));
                Prijs += discountedPrice;
            }
            ViewBag.User = _user;
            ViewBag.Price = Prijs;

            
            await _orders.CreateOrderAsync(user,ids);
            await _users.ClearShoppingListAfterOrder(user.UserId);
            await _users.UpdateUserAsync(user);
            await _users.SaveChangesAsync();

            return View("Order",items);
        }









    }
}
