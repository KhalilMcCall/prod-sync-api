
public class ProductService : IProductService
{
    private readonly ProdSyncContext _context;

    public ProductService(ProdSyncContext context)
    {
        _context = context;
    }
    public Product CreateProduct(Product product)
    {
        return product;
    }

    public Product DeleteProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public List<Product> GetProducts()
    {
        throw new NotImplementedException();
    }

    public Product UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}