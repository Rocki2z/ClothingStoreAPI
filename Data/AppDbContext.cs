using ClothingStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            entity.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .IsRequired();
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            entity.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .IsRequired();
        });
    }
}