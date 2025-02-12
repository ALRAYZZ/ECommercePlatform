namespace ShoppingCart.Models
{
	public class AddToCartRequest
	{

		public string UserId { get; set; }
		public string ProductId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}

	public class RemoveFromCartRequest
	{
		public string UserId { get; set; }
		public string ProductId { get; set; }
	}

	public class UpdateQuantityRequest
	{
		public string UserId { get; set; }
		public string ProductId { get; set; }
		public int NewQuantity { get; set; }
	}

	public class ClearCartRequest
	{
		public string UserId { get; set; }
	}
}
