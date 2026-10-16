using Microsoft.EntityFrameworkCore;
using MyPantry.Core.Entities;

namespace MyPantry.Infrastructure.Data;

public class MyPantryDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public MyPantryDbContext(DbContextOptions<MyPantryDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(p => p.Description)
                .HasMaxLength(1000);
        });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Milk", Description = "Fresh milk from the local farm", IsAvailable = true, IsExpired = false },
            new Product { Id = 2, Name = "Rice", Description = "White rice for daily meals", IsAvailable = true, IsExpired = false },
            new Product { Id = 3, Name = "Tomato Sauce", Description = "Sauce for pasta", IsAvailable = false, IsExpired = true }
        );

        base.OnModelCreating(modelBuilder);
    }
}
