using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IOrderRepo
    {
        Task< Order?> Get(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);

        Task AddOrderAsync(Order order);
      
    }
}
