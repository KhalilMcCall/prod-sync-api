using ErrorOr;

public static partial class Errors
{
    public static class User
    {
        public static Error Validation => Error.Validation(
           code: "User.Validation",
           description: "Wrong Username or Password"
       );
        public static Error UsernameExists => Error.Conflict(
            code: "Username.Exists",
            description: "Username already exists"
        );
    }
}