using Microsoft.AspNetCore.Identity.UI.Services;
using MyClientProject.Models;
using MyClientProject.Repos.Interfaces;
using MyClientProject.Services.Interfaces;

namespace MyClientProject.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepo _itemRepo;

        public ItemService(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task<Item> GetAsync(int id)
        {
            return await _itemRepo.GetAsync(id);
        }
        public async Task<bool> UpdateItemAsync(int itemId, Item updatedItem)
        {
            var existingItem = await _itemRepo.GetAsync(itemId);
           

            // Apply changes from the updated item
            existingItem.Name = updatedItem.Name;
            existingItem.Category = updatedItem.Category;
            existingItem.Price = updatedItem.Price;
            existingItem.Weight = updatedItem.Weight;
            existingItem.DeliveryDays = updatedItem.DeliveryDays;
            existingItem.StockQuantity = updatedItem.StockQuantity;
            existingItem.Description = updatedItem.Description;

            // Save changes
            await _itemRepo.UpdateItemAsync(existingItem);
            return true;
        }
        public List<Item> GetItemsByIds(List<int> list)
        {
            return _itemRepo.GetAll()
                    .Where(x => list.Contains(x.ItemId))
                    .ToList();
        }
        public List<Item> GetAllItems()
        {
            return _itemRepo.GetAll();
        }

        public async Task<bool> CreateAsync(Item item)
        {
            await _itemRepo.CreateAsync(item);
            return true;
        }
    }
}
