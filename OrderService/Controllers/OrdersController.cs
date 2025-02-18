using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTOs;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly IOrdersService _ordersService;

		public OrdersController(IOrdersService ordersService)
		{
			_ordersService=ordersService;
		}

		[HttpGet]
		public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] string? userId = null)
		{
			var orders = await _ordersService.GetAllOrdersAsync(userId);
			return Ok(orders);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderDto>> GetOrder(int id)
		{
			var order = await _ordersService.GetOrderByIdAsync(id);
			if (order == null)
			{
				return NotFound();
			}
			return Ok(order);
		}

		[HttpPost]
		public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
		{
			var createdOrder = await _ordersService.CreateOrderAsync(createOrderDto);
			return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
		}

		[HttpPut("{id}/staus")]
		public async Task<ActionResult> UpdateOrderStatus(int id, [FromQuery] string newStatus)
		{
			await _ordersService.UpdateOrderStatusAsync(id, newStatus);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			await _ordersService.DeleteOrderAsync(id);
			return NoContent();
		}
	}
}
