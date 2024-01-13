using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public string ProductModel { get; set; } = null!;
    [Range(100000, 999999)]
    public int SKU { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}