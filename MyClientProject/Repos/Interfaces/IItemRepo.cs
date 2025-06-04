using MyClientProject.Models;

namespace MyClientProject.Repos.Interfaces
{
    public interface IItemRepo
    {
        Item? Get(int id);
        IEnumerable<Item> GetAll();
    }
}
