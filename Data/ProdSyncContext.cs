
using Microsoft.EntityFrameworkCore;

public class ProdSyncContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductModel> ProductModels { get; set; } = null!;
    public DbSet<SKU> SKUs { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;

    public ProdSyncContext(DbContextOptions<ProdSyncContext> options)
            : base(options)
    {
        Console.WriteLine("Context Options Added!");

    }

    public ProdSyncContext()
    {
        Console.WriteLine("Context instance Added!");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //Category 'code' is unique
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Code)
            .IsUnique();

        //Product and SKU have a 1:1 relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.SKU)
            .WithOne(s => s.Product)
            .HasForeignKey<SKU>(s => s.ProductId);

        //SKU 'code' Column is Unique 
        modelBuilder.Entity<SKU>()
            .HasIndex(s => s.Code)
            .IsUnique();

        //User 'username' column is Unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        //UserRole 'code' is unique
        modelBuilder.Entity<UserRole>()
            .HasIndex(u => u.Code)
            .IsUnique();

    }



}