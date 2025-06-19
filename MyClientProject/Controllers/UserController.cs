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
        private readonly IOrderService _orders;
        private User _user;


        public UserController(IUserService _users,IItemService _items,IOrderService _orders)
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
            User();
            if (_user.Role == "Guest")
            {
                return _user;
            }
            else
            {
                var userId = _user.UserId;
                if (userId == null)
                    return null;

                return await _users.GetUserByIdAsync(userId);
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();

            bool isGuest = false;
            if (user.Role == "Guest")
            {
                isGuest = true;
            }
            if (user == null)
            {
                // Try load from session for guest user
                var userJson = HttpContext.Session.GetString("User");
                if (string.IsNullOrEmpty(userJson))
                {
                    // No user found at all, redirect or show error
                    return RedirectToAction("Index", "Home");
                }
                user = JsonSerializer.Deserialize<User>(userJson);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                isGuest = true;
            }

            // Get shopping list depending on user type
            List<Item> shoppingList = new List<Item>();

            if (!isGuest)
            {
                // Logged-in user, get shopping list from DB
                var shoppingListIds = await _users.GetShoppingListFromUser(user.UserId);
                foreach (var itemEntry in shoppingListIds)
                {
                    var item = await _items.GetAsync(itemEntry.ItemId);
                    if (item != null)
                    {
                        shoppingList.Add(item);
                    }
                }
            }
            else
            {
                // Guest user, shopping list is stored in user object in session
                if (user.ShoppingList != null)
                {
                    foreach (var itemId in user.ShoppingList)
                    {
                        var item = await _items.GetAsync(itemId);
                        if (item != null)
                        {
                            shoppingList.Add(item);
                        }
                    }
                }
            }

            // Calculate total price with discount
            decimal totalPrice = 0;
            decimal discountFactor = 1 - ((decimal)(user.Discount) / 100);

            foreach (var item in shoppingList)
            {
                totalPrice += item.Price * discountFactor;
            }

            ViewBag.User = user;
            ViewBag.Price = totalPrice;

            return View(shoppingList);
        }
        [HttpGet]
        public async Task<IActionResult> AddToShoppingList(int itemId)
        {
            // Try to load the current user from the database
            var user = await GetUserAsync();

            bool isGuest = false;
            if (user.Role == "Guest")
            {
                isGuest = true;
            }

            // If user not found in DB, try to get from session (guest user)
            if (user == null)
            {
                var userJson = HttpContext.Session.GetString("User");
                if (string.IsNullOrEmpty(userJson))
                {
                    return NotFound();
                }
                user = JsonSerializer.Deserialize<User>(userJson);
                if (user == null)
                {
                    return NotFound();
                }
                isGuest = true;
            }

            // Initialize shopping list if null or empty
            if (user.ShoppingList == null)
            {
                user.ShoppingList = new List<int>();
            }

            var item = await _items.GetAsync(itemId);
            if (item == null || item.StockQuantity <= 0)
            {
                return NotFound();
            }

            // Add item ID to shopping list
            user.ShoppingList.Add(item.ItemId);

            // Decrement stock quantity
            item.StockQuantity--;

            // Update item in repository
            await _items.UpdateItemAsync(item.ItemId, item);

            if (!isGuest)
            {
                // Persist user shopping list changes in DB for logged-in users
                await _users.UpdateUserAsync(user);
                await _users.SaveChangesAsync();
            }
            else
            {
                // For guests, update user in session only
                var updatedUserJson = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString("User", updatedUserJson);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var user = await GetUserAsync();

            bool isGuest = false;
            if (user.Role == "Guest")
            {
                isGuest = true;
            }
            if (user == null)
            {
                return NotFound();
            }


            var item = await _items.GetAsync(itemId); // Assuming GetAsync is async
            if (item == null)
            {
                return NotFound();
            }


            if (user.ShoppingList.Contains(item.ItemId))
            {
                user.ShoppingList.Remove(item.ItemId);
            }
                
            // Decrement stock quantity
            item.StockQuantity++;
            if (!isGuest)
            {
                // For logged-in users, update and persist user in DB
                await _users.UpdateUserAsync(user);
                await _users.SaveChangesAsync();
            }
            else
            {
                // For guests, update user in session only
                var updatedJson = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString("User", updatedJson);
            }


            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Order(Order order)
        {
            // Try to get user from DB
            var user = await GetUserAsync();

            bool isGuest = false;
            if (user.Role == "Guest")
            {
                isGuest = true;
            }
            if (user == null)
            {
                // Try get user from session (guest)
                var userJson = HttpContext.Session.GetString("User");
                if (string.IsNullOrEmpty(userJson))
                {
                    return NotFound();
                }
                user = JsonSerializer.Deserialize<User>(userJson);
                if (user == null)
                {
                    return NotFound();
                }
                isGuest = true;
            }

            List<int> shoppingListIds = new List<int>();
            List<Item> items = new List<Item>();

            if (!isGuest)
            {
                // Logged-in user, get shopping list from DB
                shoppingListIds = await _users.GetShoppingListIdsFromUser(user.UserId);
                items = await _users.GetShoppingListFromUser(user.UserId);
            }
            else
            {
                // Guest user, shopping list stored in session user object
                if (user.ShoppingList != null)
                {
                    shoppingListIds = user.ShoppingList;

                    // Load item details for each item ID
                    foreach (var id in shoppingListIds)
                    {
                        var item = await _items.GetAsync(id);
                        if (item != null)
                        {
                            items.Add(item);
                        }
                    }
                }
            }

            // Calculate total price with discount
            decimal totalPrice = 0;
            decimal discountFactor = 1 - ((decimal)user.Discount / 100);
            foreach (var item in items)
            {
                totalPrice += item.Price * discountFactor;
            }

            ViewBag.User = user;
            ViewBag.Price = totalPrice;

            // Create the order
            await _orders.CreateOrderAsync(user, shoppingListIds);

            if (!isGuest)
            {
                // Clear shopping list from DB for logged-in user
                await _users.ClearShoppingListAfterOrder(user.UserId);
                await _users.UpdateUserAsync(user);
                await _users.SaveChangesAsync();
            }
            else
            {
                // Clear guest shopping list in session user object
                user.ShoppingList.Clear();
                // Update session
                var updatedUserJson = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString("User", updatedUserJson);
            }

            return View("Order", items);
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            User(); // assuming this sets _user from session or context

            if (_user == null)
            {
                return RedirectToAction("Index", "Home"); // or any fallback if no user found
            }

            var currentUser = await _users.GetUserByIdAsync(_user.UserId);

            if (currentUser == null)
            {
                return NotFound();
            }

            return View("Profile", currentUser);
        }
        [HttpGet]
        public async Task<IActionResult> PastOrders()
        {
            // Get current user from session or context
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Index", "Home"); // or login page
            }

            var user = JsonSerializer.Deserialize<User>(userJson);
            if (user == null)
            {
                return RedirectToAction("Index", "Home"); // or login page
            }

            // Fetch orders for current user
            var orders = await _orders.GetOrdersByUserIdAsync(user.UserId);
            if (orders == null || !orders.Any())
            {
                ViewBag.Message = "Geen eerdere bestellingen gevonden.";
            }

            return View(orders); // Pass orders list to view
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            // Haal user uit session
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Index", "Home"); // Gebruiker niet ingelogd
            }

            var user = JsonSerializer.Deserialize<User>(userJson);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Haal order op uit database
            var fetchedOrder = await _orders.Get(orderId); // Zorg dat GetAsync async is
            if (fetchedOrder == null )
            {
                ViewBag.Message = "Geen bestelling gevonden of deze hoort niet bij jou.";
                return View("OrderDetail", new List<Item>()); // Lege order
            }

            var items = new List<Item>();
            foreach (var itemId in fetchedOrder.Items)
            {
                var item = await _items.GetAsync(itemId);
                if (item != null)
                {
                    items.Add(item);
                    Console.WriteLine(item.Name + "is toegevoegd");
                }
            }

            ViewBag.User = user;
            ViewBag.Order = fetchedOrder;
            return View("OrderDetail", items);
        }










    }
}
