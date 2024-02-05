using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "isAdmin")]
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
    [Authorize(Policy = "isAdmin")]
    public IActionResult AuthorizedOnly()
    {
        return Ok("Logged and authenticated");
    }

}