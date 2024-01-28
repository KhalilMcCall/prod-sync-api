using ErrorOr;

public interface IUserRolesService
{
    ErrorOr<UserRole> CreateUserRole(CreateUserRoleRequest request);
}
