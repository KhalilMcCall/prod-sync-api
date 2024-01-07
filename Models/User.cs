using System.Data.SqlTypes;
using Microsoft.AspNetCore.Identity;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public Guid UserRoleId { get; set; }
    public UserRole UserRole { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}