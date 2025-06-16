using MyClientProject.Models;

namespace MyClientProject.Services.Interfaces
{
    public interface IItemService
    {
        Task<Item> GetAsync(int id);
        Task<bool> UpdateItemAsync(int itemId, Item updatedItem);
        List<Item> GetItemsByIds(List<int> list);
        List<Item> GetAllItems();
    }
}
