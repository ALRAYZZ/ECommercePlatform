namespace OrderService.DTOs
{
	public class OrderDto
	{
		public int Id { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Total { get; set; }
		public DateTime OrderDate { get; set; }
		public string OrderStatus { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
	}
}
