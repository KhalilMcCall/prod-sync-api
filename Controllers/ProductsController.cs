using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        var pResult = _productService.CreateProduct(p);

        if (pResult.IsError && pResult.FirstError == Errors.Category.NotFound)
        {
            return NotFound(pResult.FirstError);
        }


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
        return CreatedAtAction(nameof(Post), response);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var products = _productService.GetProducts();
        return Ok(products);
    }

    [HttpGet]
    [Route("GetById")]
    public IActionResult Get(Guid id)
    {
        var pr = _productService.GetProduct(id);
        if (pr.IsError && pr.FirstError == Errors.Product.NotFound)
        {
            return NotFound(pr.FirstError);
        }

        var p = pr.Value;

        var response = new GetProductByIdResponse(
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

    [HttpPut]
    public IActionResult Put(Guid id, UpdateProductRequest request)
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

        var pr = _productService.UpdateProduct(id, p);

        if (pr.IsError)
        {
            return NotFound(pr.FirstError);
        }
        var pResult = pr.Value;

        //Generate Response
        var response = new UpdateProductResponse(
            pResult.Id,
            pResult.Name,
            pResult.Description,
            pResult.QuantityInStock,
            pResult.ProductModel,
            pResult.SKU,
            pResult.CategoryCode,
            pResult.Price,
            pResult.CreatedDate,
            pResult.LastModifiedDate
        );
        return Ok(response);
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        var pr = _productService.DeleteProduct(id);

        if (pr.IsError && pr.FirstError == Errors.Product.NotFound)
        {
            return NotFound(pr.FirstError);
        }
        return NoContent();
    }

}