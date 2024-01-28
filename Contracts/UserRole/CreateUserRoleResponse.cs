public record CreateUserRoleResponse(
    Guid Id,
    string Name,
    string Description,
    int Code,
    DateTime LastModifiedDate,
    DateTime CreattedDate
);