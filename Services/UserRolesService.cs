using ErrorOr;

public class UserRolesService : IUserRolesService
{
    private readonly ProdSyncContext _context;

    public UserRolesService(ProdSyncContext context)
    {
        _context = context;
    }
    public ErrorOr<UserRole> CreateUserRole(CreateUserRoleRequest request)
    {

        var role = _context.UserRoles.FirstOrDefault(x => x.Code == request.Code);
        if (role != null)
        {
            return Errors.UserRole.Exists;
        }

        var d = DateTime.UtcNow;
        var newUserRole = new UserRole()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Code = request.Code,
            LastModifiedDate = d,
            CreatedDate = d
        };

        _context.UserRoles.Add(newUserRole);
        _context.SaveChanges();

        return newUserRole;
    }
}