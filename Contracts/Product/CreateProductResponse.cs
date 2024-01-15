public record CreateProductResponse(
Guid Id,
string Name,
string Description,
int QuantityInStock,
string ProductModel,
int SKU,
string CategoryCode,
decimal Price,
DateTime CreatedDate,
DateTime LastModifiedDate
);
