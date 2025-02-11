using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly OrdersContext _context;

		public OrdersController(OrdersContext context)
		{
			_context=context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<OrdersModel>>> GetOrders()
		{
			return await _context.Orders.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrdersModel>> GetOrder(int id)
		{
			var result = await _context.Orders.FindAsync(id);
			if (result == null)
			{
				return NotFound();
			}
			return result;
		}

		[HttpPost]
		public async Task<ActionResult<OrdersModel>> PostOrder(OrdersModel order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetOrder", new { id = order.Id }, order);
		}
	}
}
