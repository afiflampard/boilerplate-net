using Boilerplate.Data;

namespace Boilerplate.Model.Seeder
{
    public class UserSeeder
    {
        public static void Seed(IServiceProvider services)
        {

            using var context = services.GetRequiredService<AppDBContext>();

            if (!context.Users.Any())
            {

                context.Users.Add(new User
                {
                    Username = "afif",
                    Password = BCrypt.Net.BCrypt.HashPassword("fifa123")
                });

            }
            if (!context.Products.Any())
            {
                var products = new List<Product>{
                    new Product{Name = "product-A", Price= 5000 },
                    new Product{Name = "product-B", Price= 10000},
                    new Product{Name = "product-C", Price = 15000}
                };
                context.Products.AddRange(products);
            }
            context.SaveChanges();
        }
    }
}