using Microsoft.Identity.Client;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;

namespace MyClientProject.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ShopDbContext context;

        public OrderRepo(ShopDbContext context)
        {
            this.context = context;
        }

        public Order? Get(int id)
        {
            return context.Orders.FirstOrDefault(x => x.OrderId == id);
        }

        public IEnumerable<Order> GetAllFromUser(int id)
        {
            return context.Orders.Where(x => x.UserId == id).ToList(); ;
        }

        public async Task AddOrderAsync(Order order)
        {
     
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
    }
}
