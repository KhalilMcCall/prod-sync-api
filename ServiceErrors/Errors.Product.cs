using ErrorOr;
public static partial class Errors
{
    public static class Product
    {
        public static Error NotFound => Error.NotFound(
            code: "Product.NotFound",
            description: "Product not found"
        );
        public static Error SKUExists => Error.NotFound(
            code: "Product.SKU.Exists",
            description: "Product SKU Already exists on another Product."
        );
    }
}