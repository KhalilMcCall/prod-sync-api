public interface IProductService
{
    Product CreateProduct(Product product);
    List<Product> GetProducts();

    Product UpdateProduct(Product product);

    Product DeleteProduct(Product product);
}