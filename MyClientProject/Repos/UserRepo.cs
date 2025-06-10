using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;

namespace MyClientProject.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ShopDbContext _context;

        public UserRepo(ShopDbContext context)
        {
            this._context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public async Task<List<Item>> GetShoppingListFromUser(int userId)
        {
            

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null || user.ShoppingList == null || !user.ShoppingList.Any())
                return new List<Item>();

            var shoppingListIds = user.ShoppingList;

            // Count how many times each ItemId appears
            var itemCounts = shoppingListIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            // Fetch all items with IDs in the list
            var items = await _context.Items
                .Where(item => itemCounts.Keys.Contains(item.ItemId))
                .ToListAsync();

            // Create a list to hold the repeated items
            var result = new List<Item>();

            // For each item, add it multiple times based on its count
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
    }
}
