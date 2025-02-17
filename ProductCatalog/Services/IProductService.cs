using ProductCatalog.DTOs;

namespace ProductCatalog.Services
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProductsAsync(int page = 1, int pageSize = 10, string? category = null);
		Task<ProductDto?> GetProductByIdAsync(int id);
		Task AddProductAsync(ProductDto product);
		Task UpdateProductAsync(ProductDto product);
		Task DeleteProductAsync(int id);
	}
}
