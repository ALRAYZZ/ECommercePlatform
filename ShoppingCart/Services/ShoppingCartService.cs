using Microsoft.Extensions.Options;
using ShoppingCart.Models;
using StackExchange.Redis;
using Newtonsoft.Json;
using ShoppingCart.Settings;

namespace ShoppingCart.Services
{
	public class ShoppingCartService
	{
		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _database;


		public ShoppingCartService(IOptions<RedisSettings> redisSettings)
		{
			var redisConnectionString = $"{redisSettings.Value.Host}:{redisSettings.Value.Port}";
			_redis = ConnectionMultiplexer.Connect(redisConnectionString);
			_database = _redis.GetDatabase();
		}

		public void AddToCart(string userId, string productId, string name, decimal price, int quantity)
		{
			var cartKey = $"cart:{userId}";
			var cartItem = new CartItemModel()
			{
				ProductId = productId,
				Name = name,
				Price = price,
				Quantity = quantity
			};


			var cartItemJson = JsonConvert.SerializeObject(cartItem);
			// Add the product to the cart using hash set so we can update the quantity if the product is already in the cart
			_database.HashSet(cartKey, productId, cartItemJson);
		}


		public void RemoveFromCart(string userId, string productId)
		{
			var cartKey = $"cart:{userId}";
			_database.HashDelete(cartKey, productId);
		}

		public void UpdateQuantity(string userId, string productId, int newQuantity)
		{
			var cartKey = $"cart:{userId}";
			if (_database.HashExists(cartKey, productId))
			{
				var cartItemJson = _database.HashGet(cartKey, productId);
				if (!string.IsNullOrEmpty(cartItemJson))
				{
					var cartItem = JsonConvert.DeserializeObject<CartItemModel>(cartItemJson);
					if (cartItem != null)
					{
						cartItem.Quantity = newQuantity;
						_database.HashDelete(cartKey, productId);
						_database.HashSet(cartKey, productId, JsonConvert.SerializeObject(cartItem));
					}
				}
			}
		}

		public Dictionary<string, CartItemModel> GetCart(string userId)
		{
			var cartKey = $"cart:{userId}";
			var cartItems = _database.HashGetAll(cartKey)
						   .ToDictionary(
								entry => (string)entry.Name!,
								entry => JsonConvert.DeserializeObject<CartItemModel>(entry.Value) ?? new CartItemModel()
								);

			return cartItems;
		}

		public void ClearCart(string userId)
		{
			var cartKey = $"cart:{userId}";
			_database.KeyDelete(cartKey);
		}
	}

	
}
