using MyClientProject.Models;

namespace MyClientProject.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(User user, List<int> items);
    }
}
