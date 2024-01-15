using Microsoft.AspNetCore.Authorization.Infrastructure;
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

    [HttpPost]
    public IActionResult Post(CreateProductRequest request)
    {
        //Create new Product
        var p = new Product
        {
            Name = request.Name,
            Description = request.Description,
            QuantityInStock = request.QuantityInStock,
            ProductModel = request.ProductModel,
            SKU = request.SKU,
            CategoryCode = request.CategoryCode,
            Price = request.Price
        };

        _productService.CreateProduct(p);

        //Generate Response
        var response = new CreateProductResponse(
            p.Id,
            p.Name,
            p.Description,
            p.QuantityInStock,
            p.ProductModel,
            p.SKU,
            p.CategoryCode,
            p.Price,
            p.CreatedDate,
            p.LastModifiedDate

        );
        return Ok(response);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var products = _productService.GetProducts();
        return Ok(products);
    }

    [HttpPut]
    public IActionResult Put(Guid id, Product product)
    {
        _productService.UpdateProduct(id, product);
        return Ok(product);
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        _productService.DeleteProduct(id);

        return Ok(id);
    }

}