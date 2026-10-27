using ClothingStoreAPI.DTOs;
using ClothingStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> PlaceOrder(
        CreateOrderDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderService.PlaceOrderAsync(
                dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                order);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Product not found",
                Detail = "The requested product does not exist."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Order could not be completed",
                Detail = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid order",
                Detail = ex.Message
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(
            id, cancellationToken);

        if (order is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Order not found",
                Detail = $"Order with ID {id} was not found."
            });
        }

        return Ok(order);
    }
}