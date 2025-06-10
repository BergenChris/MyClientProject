using Microsoft.AspNetCore.Mvc;
using MyClientProject.Repos;
using MyClientProject.Filters;
using MyClientProject.Models;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace MyClientProject.Controllers
{
    [KlantSessionAuthorize]
    public class ShopController:Controller
    {

        private readonly UserRepo _userRepo;
        private User _user;

        private readonly ItemRepo _itemRepo;

        public ShopController(UserRepo _userRepo, ItemRepo _itemRepo)
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
                if (_user.ShoppingList.IsNullOrEmpty())
                {
                    _user.ShoppingList = new List<int>();
                }
            }
        }

        private async Task<User> GetUserAsync()
        {
            // Replace with your actual method of fetching user from database
            // e.g., based on logged-in user identity
            User();
            var userId = _user.UserId;
            if (userId == null)
                return null;

            return await _userRepo.GetUserByIdAsync(userId);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User();
            
            var shoppingList = await _userRepo.GetShoppingListFromUser(_user.UserId);
            decimal Prijs = 0;
            foreach (var item in shoppingList)
            {
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

            var item = await _itemRepo.GetAsync(itemId); // Assuming GetAsync is async
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
                await _itemRepo.UpdateItemAsync(item);
                await _userRepo.UpdateUserAsync(user);
                // Save changes to user
                await _userRepo.SaveChangesAsync();

            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Index");
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

            var item = await _itemRepo.GetAsync(itemId); // Assuming GetAsync is async
            if (item == null)
            {
                return NotFound();
            }
            // Add item ID to shopping list
            user.ShoppingList.Remove(item.ItemId);
            // Decrement stock quantity
            item.StockQuantity++;
            // Update item in repository
            await _itemRepo.UpdateItemAsync(item);
            await _userRepo.UpdateUserAsync(user);
            // Save changes to user
            await _userRepo.SaveChangesAsync();


            return RedirectToAction("Index");
        }









    }
}
