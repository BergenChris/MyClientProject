using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using MyClientProject.Data;
using MyClientProject.Models;


namespace MyClientProject.Services
{
    public class DataSeeder
    {
        private readonly ShopDbContext _context;

        public DataSeeder(ShopDbContext context)
        {
            _context = context;
        }

        public void SeedItemsFromJson()
        {

            if (!_context.Users.Any())
            {
                var jsonClients = File.ReadAllText("Data/DummieData/Clients.json");
           
                var clients = JsonSerializer.Deserialize<List<Client>>(jsonClients);
                if (clients != null )
                {
                    foreach (var user in clients)
                    {

                        foreach (var address in user.ShippingAdresses)
                        {
                            _context.ShippingAdresses.Add(address);

                        }
                 
                        _context.Clients.Add(user);
                      
                    }
                   
                    _context.Clients.AddRange(clients);
                    _context.SaveChanges();
                }

                var jsonEmployers = File.ReadAllText("Data/DummieData/Employers.json");
            
                var employers = JsonSerializer.Deserialize<List<Employer>>(jsonEmployers);
                if (employers != null )
                {
                    foreach (var user in employers)
                    {
                       
                        _context.Employees.Add(user);
                    }
                   
                    _context.Employees.AddRange(employers);
                    _context.SaveChanges();
                }
            }


            if (!_context.Items.Any())
            {
                var jsonItems = File.ReadAllText("Data/DummieData/Items.json");
                var items = JsonSerializer.Deserialize<List<Item>>(jsonItems);
                if (items != null)
                {
                    _context.Items.AddRange(items);
                    _context.SaveChanges();
                }
            }

            if (!_context.Stores.Any())
            {
                var jsonStores = File.ReadAllText("Data/DummieData/Store.json");
                var stores = JsonSerializer.Deserialize<List<Store>>(jsonStores);
                if (stores != null)
                {
                    foreach (var store in stores)
                    {
                        foreach (var address in store.ShippingAdresses)
                        {
                            _context.ShippingAdresses.Add(address);
                        }

                    }
                    _context.Stores.AddRange(stores);
                    _context.SaveChanges();
                }
            }

            
        }
    }
}
