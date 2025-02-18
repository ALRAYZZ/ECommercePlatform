using OrderService.DTOs;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly IOrderRepository _orderRepository;

		public OrdersService(IOrderRepository orderRepository)
		{
			_orderRepository=orderRepository;
		}
		public async Task<List<OrderDto>> GetAllOrdersAsync(string? userId = null)
		{
			var orders = await _orderRepository.GetAllOrdersAsync(userId);
			return orders.Select(o => new OrderDto()
			{
				Id = o.Id,
				ProductName = o.ProductName,
				Quantity = o.Quantity,
				Price = o.Price,
				Total = o.Total,
				OrderDate = o.OrderDate,
				OrderStatus = o.OrderStatus,
				UserId = o.UserId
			}).ToList();
		}
		public async Task<OrderDto?> GetOrderByIdAsync(int id)
		{
			var order = await _orderRepository.GetOrderByIdAsync(id);
			if (order == null) return null;
			
			return new OrderDto()
			{
				Id = order.Id,
				ProductName = order.ProductName,
				Quantity = order.Quantity,
				Price = order.Price,
				Total = order.Total,
				OrderDate = order.OrderDate,
				OrderStatus = order.OrderStatus,
				UserId = order.UserId
			};
		}
		public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var order = new OrdersModel()
			{
				ProductName = createOrderDto.ProductName,
				Quantity = createOrderDto.Quantity,
				Price = createOrderDto.Price,
				UserId = createOrderDto.UserId
			};
			await _orderRepository.AddOrderAsync(order);
			return new OrderDto()
			{
				Id = order.Id,
				ProductName = order.ProductName,
				Quantity = order.Quantity,
				Price = order.Price,
				Total = order.Total,
				OrderDate = order.OrderDate,
				OrderStatus = order.OrderStatus,
				UserId = order.UserId
			};
		}
		public async Task UpdateOrderStatusAsync(int id, string newStatus)
		{
			var order = await _orderRepository.GetOrderByIdAsync(id);
			if (order != null)
			{
				order.OrderStatus = newStatus;
				await _orderRepository.UpdateOrderAsync(order);
			}
		}

		public async Task DeleteOrderAsync(int id)
		{
			await _orderRepository.DeleteOrderAsync(id);
		}

		

		

		
	}
}
