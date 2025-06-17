using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;
using MyClientProject.Services.Interfaces;

namespace MyClientProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly ShopDbContext _context; // For querying Items

        public UserService(IUserRepo userRepo, ShopDbContext context)
        {
            _userRepo = userRepo;
            _context = context;
        }

        public async Task CreateUserAsync(User user)
        {
            await _userRepo.AddUserAsync(user);
            await _context.SaveChangesAsync();
        }
        public Task<User> GetUserByIdAsync(int userId)
        {
            return _userRepo.GetUserByIdAsync(userId);
        }

        public Task UpdateUserAsync(User user)
        {
            return _userRepo.UpdateUserAsync(user);
        }
        public async Task<List<int>> GetShoppingListIdsFromUser(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null || user.ShoppingList == null || !user.ShoppingList.Any())
                return new List<int>();

            var shoppingListIds = user.ShoppingList;

            // Count how many times each item appears
            var itemCounts = shoppingListIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            // Fetch full Item entities based on IDs
            var items = await _context.Items
                .Where(item => itemCounts.Keys.Contains(item.ItemId))
                .ToListAsync();

            var result = new List<int>();

            // Add each item as many times as it appears
            foreach (var item in items)
            {
                int count = itemCounts[item.ItemId];
                for (int i = 0; i < count; i++)
                {
                    result.Add(item.ItemId);
                }
            }

            return result;
        }
        public async Task<List<Item>> GetShoppingListFromUser(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null || user.ShoppingList == null || !user.ShoppingList.Any())
                return new List<Item>();

            var shoppingListIds = user.ShoppingList;

            // Count how many times each item appears
            var itemCounts = shoppingListIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            // Fetch full Item entities based on IDs
            var items = await _context.Items
                .Where(item => itemCounts.Keys.Contains(item.ItemId))
                .ToListAsync();

            var result = new List<Item>();

            // Add each item as many times as it appears
            foreach (var item in items)
            {
                int count = itemCounts[item.ItemId];
                for (int i = 0; i < count; i++)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public async Task ClearShoppingListAfterOrder(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.ShoppingList.Clear();
                await _userRepo.UpdateUserAsync(user);
                await _userRepo.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public List<User> GetAll()
        {
              return _context.Users.ToList(); ;
        }
    }
}
