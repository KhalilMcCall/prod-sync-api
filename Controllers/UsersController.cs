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
    public IActionResult Post(LoginRequest request)
    {
        var login = _identityService.Login(request);
        if (login.IsError)
        {

            return BadRequest("Wrong Username or Password");
        }
        Response.Headers["Authorization"] = "973454753483";
        return Ok("LogIn Successful!");
    }

}