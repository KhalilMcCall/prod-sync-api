public record DeleteProductResponse(
Guid Id,
string Name,
string Description,
int QuantityInStock,
string ProductModel,
int SKU,
string CategoryCode,
decimal Price
);