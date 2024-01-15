
public class ProductService : IProductService
{
    private readonly ProdSyncContext _context;

    public ProductService(ProdSyncContext context)
    {
        _context = context;
    }
    public void CreateProduct(Product product)
    {

        //Find Category 
        var catCode = product.CategoryCode.ToLower();
        var category = _context.Categories.FirstOrDefault(x => x.Code == catCode);
        if (category != null)
        {

            product.CategoryId = category.Id;
            product.CategoryCode = category.Code;

            var d = DateTime.UtcNow;
            product.Id = Guid.NewGuid();
            product.CreatedDate = d;
            product.LastModifiedDate = d;

            //_context.Products.Add(product);
            // _context.SaveChanges();
        }
    }

    public List<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public void UpdateProduct(Guid Id, Product product)
    {
        var p = _context.Products.SingleOrDefault(x => x.Id == Id);

        if (p != null)
        {

            var d = DateTime.UtcNow;
            product.LastModifiedDate = d;
            p.Name = product.Name;
            p.Description = product.Description;
            p.ProductModel = product.ProductModel;
            p.QuantityInStock = product.QuantityInStock;

            if (p.CategoryCode != product.CategoryCode)
            {
                var c = _context.Categories.SingleOrDefault(x => x.Code == p.CategoryCode);
                if (c != null)
                {
                    p.CategoryId = c.Id;
                    p.CategoryCode = c.Code;
                }
            }

            if (p.SKU != product.SKU)
            {
                var productBySku = _context.Products.Where(x => x.SKU == product.SKU && x.Id != p.Id);
                if (productBySku == null)
                {
                    p.SKU = product.SKU;
                }
            }
            _context.Products.Update(product);
            _context.SaveChanges();
        }

    }

    public void DeleteProduct(Guid Id)
    {
        var p = _context.Products.FirstOrDefault(x => x.Id == Id);

        if (p != null)
        {
            Console.WriteLine(p);
            // _context.Products.Remove(p);
            //_context.SaveChanges();
        }
    }

}