using Boilerplate.Data;

namespace Boilerplate.Model.Seeder
{
    public class UserSeeder{
        public static void Seed(IServiceProvider services){
            
            using var context = services.GetRequiredService<AppDBContext>();

            if (!context.Users.Any()){
                
                context.Users.Add(new User{
                    Username = "afif",
                    Password = BCrypt.Net.BCrypt.HashPassword("fifa123")
                });

                context.SaveChanges();
            }
        }
    }
}