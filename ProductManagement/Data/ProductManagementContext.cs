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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Product>();
        }
    }
}
