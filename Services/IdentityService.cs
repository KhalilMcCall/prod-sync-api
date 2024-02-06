
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using Microsoft.IdentityModel.Tokens;

public class IdentityService : IIdentityService
{
    private readonly ProdSyncContext _context;

    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _passKey;
    private readonly string _jwtKey;
    public IdentityService(ProdSyncContext context, string issuer, string audience, string jwtKey, string passKey)
    {
        _context = context;
        _issuer = issuer;
        _audience = audience;
        _jwtKey = jwtKey;
        _passKey = passKey;
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
        //Security Token Descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //Subject Property sets claims
            Subject = new ClaimsIdentity(new[] {
                new Claim("Id",Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.Username),
                new Claim("isAdmin", user.UserRoleCode == "100" ? "true" : "false")
            }),

            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey)),
            SecurityAlgorithms.HmacSha256Signature)
        };

        //Token Handler
        var tokenHandler = new JwtSecurityTokenHandler();

        //Token Handler Creates Token 
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;

    }

    private byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(_passKey.Length * 2);
    }
    private byte[] GenerateHash(string password, byte[] salt)
    {
        var saltedString = password + Convert.ToHexString(salt);
        var saltedStringbytes = Encoding.UTF8.GetBytes(saltedString);
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_passKey));
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