using ProductCatalog.Models;

namespace ProductCatalog.Repositories
{
	public interface IProductRepository
	{
		Task<List<ProductModel>> GetAllProductsAsync(int page = 1, int pageSize = 10, string? category = null);
		Task<ProductModel> GetProductByIdAsync(int id);
		Task AddProductAsync(ProductModel product);
		Task UpdateProductAsync(ProductModel product);
		Task DeleteProductAsync(int id);
	}
}
