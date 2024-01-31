

using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;


public class AuthPayload
{
    public Guid id { get; set; }
    public string name { get; set; }
    public int iat { get; set; }

}
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
            UserRoleId = userRole.Id,
            UserRoleCode = request.UserRoleCode,
            LastModifiedDate = d,
            CreatedDate = d,
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();
        return newUser;

    }


    public ErrorOr<string> Login(LoginRequest request)
    {
        var u = _context.Users.FirstOrDefault(x => x.Username == request.username);
        if (u == null)
        {
            return Errors.User.Validation;
        }

        var hash = GenerateHash(request.password, u.Salt);

        var match = CompareHash(hash, u.PasswordHash);

        if (!match)
        {
            return Errors.User.Validation;
        }

        return GenerateJWT(u);
    }

    private string GenerateJWT(User user)
    {
        var header = new { alg = "HS256", typ = "JWT" };
        var now = (int)DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds();
        var payload = new { id = $"{user.Id}", name = $"{user.FirstName} {user.LastName}", iat = now };

        var headerStr = JsonSerializer.Serialize(header);
        var payloadStr = JsonSerializer.Serialize(payload);

        var headerBase64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(headerStr)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var payloadBase64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadStr)).TrimEnd('=').Replace('+', '-').Replace('/', '_'); ;

        var hashInput = $"{headerBase64Str}.{payloadBase64Str}";
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
        var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashInput));
        var signatureStr = Convert.ToBase64String(signature).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        var jwt = $"{hashInput}.{signatureStr}";

        return jwt;
    }
    public bool Authenticate(string token)
    {
        var tokenStr = token.Substring(7);
        var tokenSplit = tokenStr.Split('.');
        var header = tokenSplit[0];
        var payload = tokenSplit[1];
        var input = $"{header}.{payload}";

        Console.WriteLine($"Input: {input}");
        var signature = tokenSplit[2];

        //Hashing the header and payload 
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
        var hashedSignature = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

        //Convert the Hased signature into a Base64 string and format properly
        var inputSignature = Convert.ToBase64String(hashedSignature).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        if (inputSignature != signature)
        {
            return false;
        }


        //Decode Base64 string
        var decodedPayload = Convert.FromBase64String(payload);
        var decodedPayloadString = Encoding.UTF8.GetString(decodedPayload);

        //Deserialize
        var payloadObj = JsonSerializer.Deserialize<AuthPayload>(decodedPayloadString);
        if (payloadObj?.iat == null)
        {
            return false;
        }


        if (DateTimeOffset.UtcNow > DateTimeOffset.FromUnixTimeSeconds(payloadObj.iat))
        {
            return false;
        }

        return true;
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
        if (hash1 == null || hash2 == null || !hash1.SequenceEqual(hash2))
        {
            return false;
        }
        return true;
    }


}