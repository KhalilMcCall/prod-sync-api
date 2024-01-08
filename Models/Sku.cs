
using System.ComponentModel.DataAnnotations.Schema;

public class SKU
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}
