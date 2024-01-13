
public class ProductService : IProductService
{
    private readonly ProdSyncContext _context;

    public ProductService(ProdSyncContext context)
    {
        _context = context;
    }
    public void CreateProduct(Product product)
    {
        var d = DateTime.UtcNow;
        product.Id = Guid.NewGuid();
        product.CreatedDate = d;
        product.LastModifiedDate = d;

        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    public List<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public void UpdateProduct(Product product)
    {
        var d = DateTime.UtcNow;
        product.LastModifiedDate = d;
        _context.Products.Update(product);
        _context.SaveChanges();
    }
}