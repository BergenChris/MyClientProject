using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;
using MyClientProject.Services.Interfaces;

namespace MyClientProject.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IItemRepo _itemRepo; // if needed for validation, etc.

        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<Order> CreateOrderAsync(User user, List<int> items)
        {
            // Business logic (validation, total calculation)
            var order = new Order
            {
                UserId = user.UserId,
                Items = items,
                OrderDate = DateTime.UtcNow,

            };

            await _orderRepo.AddOrderAsync(order);
            return order;
        }
    }
}
