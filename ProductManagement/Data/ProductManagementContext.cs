using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Client>()
                .OwnsMany(c => c.Addresses, e =>
                {
                    e.WithOwner().HasForeignKey("UserId");
                    e.HasKey("UserId", "AddressId");
                });
            modelBuilder.Entity<User>().Property(u => u.RegistrationDate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Product>().Property(p => p.Stock)
                .HasDefaultValue(0);
            modelBuilder.Entity<Request>()
                .OwnsOne(p => p.DeliveryAddress, e =>
                {
                    e.Ignore(e => e.AddressId);
                    e.Ignore(e => e.Selected);
                    e.ToTable("Request");
                });
            modelBuilder.Entity<ItemOrder>()
                .HasKey(ip => new { ip.RequestId, ip.ProductId });
        }
    }
}
