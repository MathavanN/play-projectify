using Microsoft.AspNetCore.Mvc;

namespace Service.Product.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(new { Id = id, Name = $"Product {id}" });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(
            new[]
            {
                new { Id = 1, Name = "Product 1" },
                new { Id = 2, Name = "Product 2" }
            });
    }
}
