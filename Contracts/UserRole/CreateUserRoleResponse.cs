public record CreateUserRoleResponse(
    Guid Id,
    string Name,
    string Description,
    string Code,
    DateTime LastModifiedDate,
    DateTime CreattedDate
);