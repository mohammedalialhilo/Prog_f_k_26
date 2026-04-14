using Microsoft.EntityFrameworkCore;

public class BakeryContext : DbContext
{
    public BakeryContext(DbContextOptions<BakeryContext> options) : base(options)
    {
    }

    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<SupplierProduct> SupplierProducts => Set<SupplierProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupplierProduct>()
            .HasKey(sp => new { sp.SupplierId, sp.ProductId });

        modelBuilder.Entity<SupplierProduct>()
            .HasOne(sp => sp.Supplier)
            .WithMany(s => s.SupplierProducts)
            .HasForeignKey(sp => sp.SupplierId);

        modelBuilder.Entity<SupplierProduct>()
            .HasOne(sp => sp.Product)
            .WithMany(p => p.SupplierProducts)
            .HasForeignKey(sp => sp.ProductId);
    }
}