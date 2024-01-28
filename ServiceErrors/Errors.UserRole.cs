using ErrorOr;

public static partial class Errors
{
    public static class UserRole
    {
        public static Error NotFound => Error.NotFound(
            code: "UserRole.NotFound",
            description: "UserRole not found"
        );
        public static Error Exists => Error.Conflict(
    code: "UserRole.Exists",
    description: "UserRole Code already exists"
);
    }
}