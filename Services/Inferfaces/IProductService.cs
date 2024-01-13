public interface IProductService
{
    void CreateProduct(Product product);
    List<Product> GetProducts();

    void UpdateProduct(Product product);

    void DeleteProduct(Product product);
}