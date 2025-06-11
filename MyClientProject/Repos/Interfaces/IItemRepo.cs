using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IItemRepo
    {
        Task<Item> GetAsync(int id);
        Task UpdateItemAsync(Item item);
        List<Item> GetAll();
       
        

    }
}
