using ErrorOr;

public interface IIdentityService
{

    ErrorOr<User> CreateUser(CreateUserRequest request);
    // User GetUser(CreateUserRequest request);
    // User UpdateUser(CreateUserRequest request);
    ErrorOr<string> Login(LoginRequest request);
    bool Authenticate(string token);
}