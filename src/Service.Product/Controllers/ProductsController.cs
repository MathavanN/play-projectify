using Microsoft.AspNetCore.Mvc;
using Service.Product.Data;

namespace Service.Product.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _productRepository.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var result = _productRepository.Get(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    //[HttpPost]
    //public IActionResult Create([FromBody] Product product)
    //{
    //    if (product == null) return BadRequest();

    //    var created = _productRepository.Add(product);

    //    // Return 201 Created with Location header pointing to GET /api/products/{id}
    //    return CreatedAtRoute(
    //        "GetProductById",
    //        new { id = created.Id },
    //        created
    //    );
    //}

    //[HttpPut("{id}")]
    //public IActionResult Update(int id, [FromBody] Product product)
    //{
    //    if (product == null || product.Id != id) return BadRequest();

    //    var existing = _productRepository.Get(id);
    //    if (existing == null) return NotFound();

    //    _productRepository.Update(product);
    //    return NoContent(); // 204 No Content
    //}

    //[HttpDelete("{id}")]
    //public IActionResult Delete(int id)
    //{
    //    var existing = _productRepository.Get(id);
    //    if (existing == null) return NotFound();

    //    _productRepository.Delete(id);
    //    return NoContent(); // 204 No Content
    //}
}