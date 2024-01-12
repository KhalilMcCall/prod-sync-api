
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductModelsController : ControllerBase
{
    private readonly IProductModelService _productModelService;

    public ProductModelsController(IProductModelService productModelService)
    {
        _productModelService = productModelService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var productModels = _productModelService.GetProductModels();

        return Ok(productModels);
    }

    public IActionResult Post(ProductModel productModel)
    {
        _productModelService.CreateProductModel(productModel);

        return Ok(productModel);
    }
}