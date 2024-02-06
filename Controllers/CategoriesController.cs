
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var categories = _categoryService.GetCategories();
        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Policy = "isAdmin")]
    public IActionResult Post(CreateCategoryRequest request)
    {
        var c = new Category()
        {
            Name = request.Name,
            Code = request.Code
        };

        var cr = _categoryService.CreateCategory(c);
        if (cr.IsError)
        {
            return BadRequest(cr.Errors);
        }
        var resObj = cr.Value;
        var response = new CreateCategoryResponse(
            resObj.Name,
            resObj.Code,
            resObj.CreatedDate,
            resObj.LastModifiedDate
        );

        return CreatedAtAction(nameof(Post), response);
    }
}