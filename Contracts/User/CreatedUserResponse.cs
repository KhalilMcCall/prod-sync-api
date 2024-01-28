public record CreateUserResponse(
    Guid Id,
    string Username,
    string FirstName,
    string LastName,
    string Email,
    int UserRoleCode,
    DateTime LastModifiedDate,
    DateTime CreattedDate
);