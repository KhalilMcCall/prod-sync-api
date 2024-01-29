using System.ComponentModel.DataAnnotations;

public record CreateUserRoleRequest(
    string Name,
    string Description,
    string Code
);