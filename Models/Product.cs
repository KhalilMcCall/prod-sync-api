public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public Guid ProductModelId { get; set; }
    public ProductModel ProductModel { get; set; } = null!;
    public Guid SkuId { get; set; }
    public Sku Sku { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}