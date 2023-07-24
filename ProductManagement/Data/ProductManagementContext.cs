using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public class ProductManagementContext : DbContext
    {
 
        public ProductManagementContext(DbContextOptions<ProductManagementContext> options)
        : base(options){}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ItemOrder> ItemOrders { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<Address>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<ItemOrder>();
            modelBuilder.Entity<Request>();
            modelBuilder.Entity<User>();
        }
    }
}
