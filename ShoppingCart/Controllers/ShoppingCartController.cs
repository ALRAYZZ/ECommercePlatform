using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ShoppingCartController : ControllerBase
	{
		private readonly ShoppingCartService _shoppingCartService;

		public ShoppingCartController(ShoppingCartService shoppingCartService)
		{
			_shoppingCartService=shoppingCartService;
		}

		[HttpPost]
		public IActionResult AddToCart([FromBody] AddToCartRequest request)
		{
			_shoppingCartService.AddToCart(request.UserId, request.ProductId, request.Name, request.Price, request.Quantity);
			return Ok("Item added to cart.");
		}

		[HttpPost("remove")]
		public IActionResult RemoveFromCart([FromBody] RemoveFromCartRequest request)
		{
			_shoppingCartService.RemoveFromCart(request.UserId, request.ProductId);
			return Ok("Item removed from cart.");
		}

		[HttpPost("update")]
		public IActionResult UpdateQuantity([FromBody] UpdateQuantityRequest request)
		{
			_shoppingCartService.UpdateQuantity(request.UserId, request.ProductId, request.NewQuantity);
			return Ok("Quantity updated.");
		}

		[HttpGet("get")]
		public IActionResult GetCart([FromQuery] string userId)
		{
			var cart = _shoppingCartService.GetCart(userId);
			return Ok(cart.Values.ToList());
		}

		[HttpPost("clear")]
		public IActionResult ClearCart([FromBody] ClearCartRequest request)
		{
			_shoppingCartService.ClearCart(request.UserId);
			return Ok("Cart cleared.");
		}
	}
}
