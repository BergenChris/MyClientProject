using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IItemRepo
    {
        Task<Item> GetAsync(int itemId);
        Task UpdateItemAsync(Item item);
        List<Item> GetAll();
        Task CreateAsync(Item item);




    }
}
