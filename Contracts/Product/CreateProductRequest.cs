public record CreateProductRequest(
string Name,
string Description,
int QuantityInStock,
string ProductModel,
int SKU,
string CategoryCode,
decimal Price
);
