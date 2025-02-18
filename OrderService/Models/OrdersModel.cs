namespace OrderService.Models
{
	public class OrdersModel
	{
		public int Id { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Total => Price * Quantity; // Calculate total dynamically
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public string OrderStatus { get; set; } = "Pending";
		public string UserId { get; set; } = string.Empty;
	}
}
