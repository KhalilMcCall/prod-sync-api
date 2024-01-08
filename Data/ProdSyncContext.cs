
using Microsoft.EntityFrameworkCore;

public class ProdSyncContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductModel> ProductModels { get; set; } = null!;
    public DbSet<SKU> SKUs { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //User 'username' column is Unique
        modelBuilder.Entity<User>()
            .HasIndex(s => s.Username)
            .IsUnique();

        //SKU 'code' Column is Unique 
        modelBuilder.Entity<SKU>()
            .HasIndex(s => s.Code)
            .IsUnique();

        //Product and SKU have a 1:1 relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.SKU)
            .WithOne(s => s.Product)
            .HasForeignKey<SKU>(s => s.ProductId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=ProdSyncDb;User Id=SA;Password=<YourStrong@Passw0rd>;MultipleActiveResultSets=True;Encrypt=False;");
    }

}