using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IIdentityService _identityService;
    public UsersController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Route("Login")]
    public IActionResult Login(LoginRequest request)
    {
        var login = _identityService.Login(request);
        if (login.IsError)
        {
            return BadRequest("Wrong Username or Password");
        }
        var userValue = login.Value;
        Response.Headers["Authorization"] = userValue;
        return Ok("LogIn Successful!");
    }

    [HttpPost]
    [Route("CreateUser")]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        var u = _identityService.CreateUser(request);

        if (u.IsError)
        {
            return BadRequest(u.Errors);
        }

        var userValue = u.Value;

        var userResult = new CreateUserResponse(
                userValue.Id,
                userValue.Username,
                userValue.FirstName,
                userValue.LastName,
                userValue.Email,
                userValue.UserRoleCode,
                userValue.LastModifiedDate,
                userValue.CreatedDate
        );

        return CreatedAtAction("CreateUser", userResult);
    }


    [HttpGet]
    [Route("AuthorizedOnly")]
    public IActionResult AuthorizedOnly()
    {
        if (!Request.Headers.ContainsKey("Authorization") || !_identityService.Authenticate(Request.Headers["Authorization"]))
        {
            return BadRequest("Not Logged In.");
        }

        return Ok("Logged and authenticated");


    }

}