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
