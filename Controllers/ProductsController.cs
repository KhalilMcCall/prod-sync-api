using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var products = _productService.GetProducts();
        return Ok(products);
    }

    [HttpPost]
    public IActionResult Post(Product product)
    {
        _productService.CreateProduct(product);
        return Ok(product);
    }

    [HttpPut]
    public IActionResult Put(Product product)
    {
        _productService.UpdateProduct(product);
        return Ok(product);
    }

    [HttpDelete]
    public IActionResult Delete(Product product)
    {
        _productService.DeleteProduct(product);
        return Ok(product);
    }

}