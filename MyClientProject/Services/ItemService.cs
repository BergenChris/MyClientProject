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
        public async Task<bool> UpdateItemAsync(int itemId)
        {
            var item = await _itemRepo.GetAsync(itemId);
            if (item == null)
                return false;

            // Business logic: validate stock, apply discounts, etc.
          
            await _itemRepo.UpdateItemAsync(item);
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
    }
}
