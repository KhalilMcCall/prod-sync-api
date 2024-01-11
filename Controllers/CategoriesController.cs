
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
    public IActionResult Post(Category category)
    {
        Console.WriteLine($"Inside Post Category: {category}");
        _categoryService.CreateCategory(category);
        return Ok(category);
    }
}