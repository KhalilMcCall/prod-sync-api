
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService)
    {
        Console.WriteLine("Categories Controller Created!");
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var categories = _categoryService.GetCategories();
        return Ok(categories);
    }

    [HttpPost]
    public IActionResult Post(CreateCategoryRequest request)
    {
        var c = new Category()
        {
            Name = request.Name,
            Code = request.Code
        };

        _categoryService.CreateCategory(c);

        var response = new CreateCategoryResponse(
            c.Name,
            c.Code,
            c.CreatedDate,
            c.LastModifiedDate
        );


        return CreatedAtAction(nameof(Post), response);
    }
}