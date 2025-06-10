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

            var shoppingItems = await _context.Items
                .Where(item => shoppingListIds.Contains(item.ItemId))
                .ToListAsync();

            return shoppingItems;
        }
    }
}
