using Boilerplate.Model;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Product>().Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
           modelBuilder.Entity<Category>().Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");
           modelBuilder.Entity<User>().Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");
        }

    }
}