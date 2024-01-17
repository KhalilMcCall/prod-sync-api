using ErrorOr;

public interface IProductService
{
    ErrorOr<Product> CreateProduct(Product product);
    List<Product> GetProducts();
    ErrorOr<Product> GetProduct(Guid id);

    ErrorOr<Product> UpdateProduct(Guid Id, Product product);

    ErrorOr<Product> DeleteProduct(Guid Id);
}