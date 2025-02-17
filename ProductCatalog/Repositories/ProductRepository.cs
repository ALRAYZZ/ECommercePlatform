using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ProductContext _context;

		public ProductRepository(ProductContext context)
		{
			_context=context;
		}

		public async Task<List<ProductModel>> GetAllProductsAsync(int page = 1, int pageSize = 10, string? category = null)
		{
			var query = _context.Products.AsQueryable(); // This is more efficient than using _context.Products.ToList()
			if (!string.IsNullOrEmpty(category))
			{
				query = query.Where(p => p.Category == category);
			}
			return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
		}

		public async Task<ProductModel> GetProductByIdAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				throw new KeyNotFoundException($"Product with id {id} not found");
			}
			return product;
		}

		public async Task AddProductAsync(ProductModel product)
		{
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductAsync(ProductModel product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
		}
	}
}
