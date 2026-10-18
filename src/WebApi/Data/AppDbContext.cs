using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Description = "15-inch developer laptop", Price = 1299.99m, Stock = 25 },
            new Product { Id = 2, Name = "Mechanical Keyboard", Description = "Hot-swappable 75% keyboard", Price = 149.00m, Stock = 100 },
            new Product { Id = 3, Name = "USB-C Dock", Description = "Dual 4K docking station", Price = 229.50m, Stock = 40 }
        );
    }
}
