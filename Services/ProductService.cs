
using System.Reflection.Metadata.Ecma335;
using ErrorOr;

public class ProductService : IProductService
{
    private readonly ProdSyncContext _context;

    public ProductService(ProdSyncContext context)
    {
        _context = context;
    }
    public ErrorOr<Product> CreateProduct(Product product)
    {

        //Find Category 
        var catCode = product.CategoryCode.ToLower();
        var c = _context.Categories.FirstOrDefault(x => x.Code == catCode);
        if (c == null)
        {
            return Errors.Category.NotFound;
        }

        var productBySku = _context.Products.FirstOrDefault(x => x.SKU == product.SKU);
        if (productBySku != null)
        {
            return Errors.Product.SKUExists;
        }

        product.CategoryId = c.Id;
        product.CategoryCode = c.Code;
        var d = DateTime.UtcNow;
        product.Id = Guid.NewGuid();
        product.CreatedDate = d;
        product.LastModifiedDate = d;

        //_context.Products.Add(product);
        // _context.SaveChanges();

        return product;
    }

    public List<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public ErrorOr<Product> GetProduct(Guid id)
    {
        var p = _context.Products.FirstOrDefault(x => x.Id == id);
        if (p != null)
        {
            return p;
        }

        return Errors.Product.NotFound;
    }

    public ErrorOr<Product> UpdateProduct(Guid Id, Product product)
    {
        var p = _context.Products.SingleOrDefault(x => x.Id == Id);

        if (p == null)
        {
            return Errors.Product.NotFound;
        }

        var d = DateTime.UtcNow;
        product.LastModifiedDate = d;
        p.Name = product.Name;
        p.Description = product.Description;
        p.ProductModel = product.ProductModel;
        p.QuantityInStock = product.QuantityInStock;

        if (p.CategoryCode != product.CategoryCode)
        {
            var c = _context.Categories.SingleOrDefault(x => x.Code == p.CategoryCode);
            if (c == null)
            {
                return Errors.Category.NotFound;
            }
            p.CategoryId = c.Id;
            p.CategoryCode = c.Code;
        }

        if (p.SKU != product.SKU)
        {
            var productBySku = _context.Products.FirstOrDefault(x => x.SKU == product.SKU && x.Id != p.Id);
            if (productBySku != null)
            {
                return Errors.Product.SKUExists;
            }
            p.SKU = product.SKU;
        }

        _context.Products.Update(p);
        _context.SaveChanges();
        return p;



    }

    public ErrorOr<Product> DeleteProduct(Guid Id)
    {
        var p = _context.Products.FirstOrDefault(x => x.Id == Id);

        if (p == null)
        {
            return Errors.Product.NotFound;
        }

        // _context.Products.Remove(p);
        //_context.SaveChanges();
        return p;
    }

}