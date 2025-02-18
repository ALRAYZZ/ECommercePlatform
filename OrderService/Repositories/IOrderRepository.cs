using OrderService.Models;

namespace OrderService.Repositories
{
	public interface IOrderRepository
	{
		Task<List<OrdersModel>> GetAllOrdersAsync(string? userId = null);
		Task<OrdersModel?> GetOrderByIdAsync(int id);
		Task AddOrderAsync(OrdersModel order);
		Task UpdateOrderAsync(OrdersModel order);
		Task DeleteOrderAsync(int id);
	}
}
