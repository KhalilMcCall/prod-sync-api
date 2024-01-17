using ErrorOr;

public static partial class Errors
{
    public static class Category
    {
        public static Error NotFound => Error.NotFound(
            code: "Category.NotFound",
            description: "Category not found"
        );
        public static Error CategoryCodeExists => Error.NotFound(
            code: "Category.Exists",
            description: "Category code already exists"
        );
    }
}