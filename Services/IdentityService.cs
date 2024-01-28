

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Configuration;

public class IdentityService : IIdentityService
{
    private readonly ProdSyncContext _context;

    private readonly string secretKey = "kobeBean";
    private readonly int SaltSize = 16;
    public IdentityService(ProdSyncContext context)
    {
        _context = context;
    }
    public ErrorOr<User> CreateUser(CreateUserRequest request)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == request.Username);
        if (user != null)
        {
            return Errors.User.UsernameExists;
        }

        var userRole = _context.UserRoles.FirstOrDefault(x => x.Code == request.UserRoleCode);
        if (userRole == null)
        {
            return Errors.UserRole.NotFound;
        }

        var d = DateTime.UtcNow;

        var salt = GenerateSalt();
        var passwordHash = GenerateHash(request.Password, salt);
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Salt = salt,
            PasswordHash = passwordHash,
            Active = true,
            UserRoleCode = request.UserRoleCode,
            LastModifiedDate = d,
            CreatedDate = d,
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();
        return newUser;

    }


    public ErrorOr<User> Login(LoginRequest request)
    {
        var u = _context.Users.FirstOrDefault(x => x.Username == request.username);
        if (u != null)
        {
            return Errors.User.Validation;
        }

        var hash = GenerateHash(request.password, u.Salt);

        var match = CompareHash(hash, u.PasswordHash);

        if (!match)
        {
            return Errors.User.Validation;
        }

        return u;
    }

    private byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(SaltSize);
    }
    private byte[] GenerateHash(string password, byte[] salt)
    {
        var saltedString = password + Convert.ToHexString(salt);
        var saltedStringbytes = Encoding.UTF8.GetBytes(saltedString);
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
        return hmac.ComputeHash(saltedStringbytes);
    }

    private bool CompareHash(byte[] hash1, byte[] hash2)
    {
        if (hash1 == null || hash2 == null)
        {
            return false;
        }
        if (!hash1.SequenceEqual(hash2))
        {
            return false;
        }
        return true;
    }

    private string GenerateToken(string password)
    {
        return "";
    }
}