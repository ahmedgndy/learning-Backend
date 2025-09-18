using Microsoft.AspNetCore.Mvc;

namespace _04_Routing_Demo_Minimal_API_with_Attribute_Conventional_Routing.Controllers;

[ApiController]
[Route("orders")]

public class OrdersController : ControllerBase
{
    //get all orders
    [HttpGet]
    public IActionResult GetAllOrders()
    {
        return Ok(new[] { "order1", "order2", "order3" });
    }

    //get all orders
    [HttpGet("{id:int:range(10,20)}")]
    public IActionResult GetAllOrdersById(int id)
    {
        return Ok(new[] { $"order{id}" });
    }

    //using custom Constraint
    [HttpGet("month/{month:int:monthConstraints}")]
    public IActionResult GetOrdersByMonth(int month)
    {
        return Ok(new[] { $"order1 from month {month}", $"order2 from month {month}", $"order3 from month {month}" });
    }

 }
