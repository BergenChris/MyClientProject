using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;

namespace MyClientProject.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ShopDbContext _context;

        public OrderRepo(ShopDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Order?> Get(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
     
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        
    }
}
