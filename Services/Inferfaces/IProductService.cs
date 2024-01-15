public interface IProductService
{
    void CreateProduct(Product product);
    List<Product> GetProducts();

    void UpdateProduct(Guid Id, Product product);

    void DeleteProduct(Guid Id);
}