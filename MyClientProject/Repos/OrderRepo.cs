using Microsoft.Identity.Client;
using MyClientProject.Data;
using MyClientProject.Models;

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
            return context.Orders.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Order> GetAllFromUser(int id)
        {
            return context.Orders.Where(x => x.User.Id == id).ToList(); ;
        }
    }
}
