using OrderService.DTOs;

namespace OrderService.Services
{
	public interface IOrdersService
	{
		Task<List<OrderDto>> GetAllOrdersAsync(string? userId = null);
		Task<OrderDto?> GetOrderByIdAsync(int id);
		Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
		Task UpdateOrderStatusAsync(int id, string newStatus);
		Task DeleteOrderAsync(int id);
	}
}
