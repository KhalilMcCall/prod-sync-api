
using Microsoft.EntityFrameworkCore;

public class ProdSyncContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;

    public ProdSyncContext(DbContextOptions<ProdSyncContext> options)
            : base(options)
    {

    }

    public ProdSyncContext()
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //Category 'code' is unique
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Code)
            .IsUnique();

        //Product and SKU have a 1:1 relationship
        modelBuilder.Entity<Product>()
            .Property(p => p.SKU)
            .IsRequired()
            .HasColumnName("SKU")
            .HasAnnotation("MinLength", 6)
            .HasAnnotation("MaxLength", 6);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
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