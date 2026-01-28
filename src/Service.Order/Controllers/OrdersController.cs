using Microsoft.AspNetCore.Mvc;

namespace Service.Order.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(new { Id = id, Total = 100 + id });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new[]
        {
        new { Id = 1, Total = 100 },
        new { Id = 2, Total = 200 }
    });
    }
}
