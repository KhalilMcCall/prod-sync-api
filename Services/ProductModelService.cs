
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

public class ProductModelService : IProductModelService
{
    private readonly ProdSyncContext _context;

    public ProductModelService(ProdSyncContext context)
    {
        _context = context;
    }

    public void CreateProductModel(ProductModel productModel)
    {
        var d = DateTime.UtcNow;
        productModel.Id = Guid.NewGuid();
        productModel.CreatedDate = d;
        productModel.LastModifiedDate = d;

        _context.Add(productModel);
        _context.SaveChanges();
    }

    public List<ProductModel> GetProductModels()
    {
        return _context.ProductModels.ToList();
    }
}