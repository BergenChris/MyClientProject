using Microsoft.EntityFrameworkCore;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;
using MyClientProject.Services.Interfaces;

namespace MyClientProject.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IItemRepo _itemRepo; // if needed for validation, etc.

        public OrderService(IOrderRepo _orderRepo, IItemRepo _itemRepo)
        {
            this._orderRepo = _orderRepo;
            this._itemRepo = _itemRepo;
        }

        public async Task<Order> CreateOrderAsync(User user, List<int> items)
        {
            var itemList = new List<Item>();
            foreach (var itemId in items)
            {
                var item = await _itemRepo.GetAsync(itemId);
                if (item != null)
                {
                    itemList.Add(item);
                }
                else
                {
                    // Optionally handle missing items here (skip or throw)
                }
            }

            // Calculate total price
            decimal totalPrice = itemList.Sum(i => i.Price);

            // Business logic (validation, total calculation)
            var order = new Order
            {
                UserId = user.UserId,
                Items = items,
                TotalPrice = totalPrice * (100 - user.Discount) / 100,
                UserEmail = user.UserEmail

            };

            await _orderRepo.AddOrderAsync(order);
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepo.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order>? Get(int orderId)
        {
            return await _orderRepo.Get(orderId);
        }


    }
}
