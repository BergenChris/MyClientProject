using MyClientProject.Models;

namespace MyClientProject.Repos
{
    public interface IItemRepo
    {
        Item? Get(int id);
        IEnumerable<Item> GetAll();
    }
}
