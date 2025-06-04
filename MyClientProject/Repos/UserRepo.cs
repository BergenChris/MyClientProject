using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;

namespace MyClientProject.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ShopDbContext context;

        public UserRepo(ShopDbContext context)
        {
            this.context = context;
        }

        public User? Get(int id)
        {
            return context.Users.FirstOrDefault(x=>x.UserId == id);
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users;
        }

        public async Task<List<Item>> GetShoppingListFromUser(int id)
        {
            var user = await context.Users.Include(x => x.ShoppingList).FirstOrDefaultAsync(x => x.UserId == id);
            return user?.ShoppingList ?? new List<Item>();
        }
    }
}
