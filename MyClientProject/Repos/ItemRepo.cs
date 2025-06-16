using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;

namespace MyClientProject.Repos
{
    public class ItemRepo : IItemRepo
    {
        private readonly ShopDbContext _context;

        public ItemRepo(ShopDbContext context)
        {
            this._context = context;
        }

        public async Task<Item> GetAsync(int itemId)
        {
            return await _context.Items.FindAsync(itemId);
        }

        public async Task UpdateItemAsync(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
        }

        public List<Item> GetAll()
        {
            return _context.Items.ToList();
        }

        public void UpdateItem(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
        }

        
    }
}
