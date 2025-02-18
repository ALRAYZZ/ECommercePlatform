using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Repositories
{
	public class OrdersRepository : IOrderRepository
	{
		private readonly OrdersContext _context;

		public OrdersRepository(OrdersContext context)
		{
			_context=context;
		}

		public async Task<List<OrdersModel>> GetAllOrdersAsync(string? userId = null)
		{
			var query = _context.Orders.AsQueryable();
			if (!string.IsNullOrEmpty(userId))
			{
				query = query.Where(o => o.UserId == userId);
			}
			return await query.ToListAsync();
		}
		public async Task<OrdersModel?> GetOrderByIdAsync(int id)
		{
			return await _context.Orders.FindAsync(id);
		}

		public async Task AddOrderAsync(OrdersModel order)
		{
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
		}
		public async Task UpdateOrderAsync(OrdersModel order)
		{
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteOrderAsync(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order != null)
			{
				_context.Orders.Remove(order);
				await _context.SaveChangesAsync();
			}
		}
	}
}
