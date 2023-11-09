using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{

    public IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("CreateOrder/{customerId}")]
    public async Task<IActionResult> CreateOrderAsync(CreateOrderRequestModel model, [FromRoute] string customerId)
    {
        var create = await _orderService.CreateOrderAsync(model, customerId);
        if (create.Status)
        {
            return Ok(create);
        }
        return BadRequest(create);
    }

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetOrdersAsync()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        if (!orders.Status)
        {
            return BadRequest(orders);
        }
        return Ok(orders);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetOrderByIdAsync([FromRoute] string id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (!order.Status)
        {
            return BadRequest(order);
        }
        return Ok(order);
    }

    [HttpGet("GetByCustomerId/{id}")]
    public async Task<IActionResult> GetOrderByCustomerIdAsync([FromRoute] string id)
    {
        var order = await _orderService.GetOrdersByCustomerIdAsync(id);
        if (!order.Status)
        {
            return BadRequest(order);
        }
        return Ok(order);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdatedOrderAsync(string id, [FromForm] UpdateOrderRequestModel model)
    {
        var order = await _orderService.UpdateOrderAsync(id, model);
        if (!order.Status)
        {
            return BadRequest(order);
        }
        return Ok(order);
    }

    [HttpGet("GetAllDeliveredOrders")]
    public async Task<IActionResult> GetAllDeleveredOrders()
    {
        var orders = await _orderService.GetAllDeliveredOrdersAsync();
        if (!orders.Status)
        {
            return BadRequest(orders);
        }
        return Ok(orders);
    }

    [HttpGet("GetAllUnDeliveredOrders")]
    public async Task<IActionResult> GetAllUnDeleveredOrders()
    {
        var orders = await _orderService.GetAllUnDeliveredOrdersAsync();
        if (!orders.Status)
        {
            return BadRequest(orders);
        }
        return Ok(orders);
    }

}
