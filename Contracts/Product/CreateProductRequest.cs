using System.ComponentModel.DataAnnotations;

public record CreateProductRequest(
string Name,
string Description,
int QuantityInStock,
string ProductModel,
[Range(100000, 999999, ErrorMessage = "SKU range must be 100000 - 999999")]
int SKU,
string CategoryCode,
[Range(0.01, 1000000, ErrorMessage = "Price range must be .01 - 1,000,000")]
decimal Price
);
