

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly IUserRolesService _userRolesService;

    public UserRolesController(IUserRolesService userRolesService)
    {
        _userRolesService = userRolesService;
    }
    [HttpPost]
    public IActionResult Post(CreateUserRoleRequest request)
    {

        var userRole = _userRolesService.CreateUserRole(request);
        if (userRole.IsError)
        {
            return BadRequest(userRole.FirstError);
        }

        var result = userRole.Value;

        var response = new CreateUserRoleResponse(
            result.Id,
            result.Name,
            result.Description,
            result.Code,
            result.LastModifiedDate,
            result.CreatedDate
        );

        return CreatedAtAction("Post", response);
    }
}


