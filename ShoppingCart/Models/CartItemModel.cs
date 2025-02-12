namespace ShoppingCart.Models
{
	public class CartItemModel
	{
		public string ProductId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
