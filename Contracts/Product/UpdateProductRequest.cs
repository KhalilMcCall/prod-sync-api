public record UpdateProductRequest(
string Name,
string Description,
int QuantityInStock,
string ProductModel,
int SKU,
string CategoryCode,
decimal Price
);