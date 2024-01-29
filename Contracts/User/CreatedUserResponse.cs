public record CreateUserResponse(
    Guid Id,
    string Username,
    string FirstName,
    string LastName,
    string Email,
    string UserRoleCode,
    DateTime LastModifiedDate,
    DateTime CreatedDate
);