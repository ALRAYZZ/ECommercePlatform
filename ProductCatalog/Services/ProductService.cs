using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories;

namespace ProductCatalog.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository=productRepository;
		}

		public async Task<List<ProductDto>> GetAllProductsAsync(int page = 1, int pageSize = 10, string? category = null)
		{
			var products = await _productRepository.GetAllProductsAsync(page, pageSize, category);
			return products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				Category = p.Category,
				Stock = p.Stock
			}).ToList();
		}
		public async Task<ProductDto?> GetProductByIdAsync(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			return product == null ? null : new ProductDto()
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Category = product.Category,
				Stock = product.Stock
			};
		}

		public async Task AddProductAsync(ProductDto productDto)
		{
			var product = new ProductModel()
			{
				Name = productDto.Name,
				Description = productDto.Description,
				Price = productDto.Price,
				Category = productDto.Category,
				Stock = productDto.Stock
			};
			await _productRepository.AddProductAsync(product);
		}
		public async Task UpdateProductAsync(ProductDto productDto)
		{
			var product = await _productRepository.GetProductByIdAsync(productDto.Id);
			if (product != null)
			{
				product.Name = productDto.Name;
				product.Description = productDto.Description;
				product.Price = productDto.Price;
				product.Category = productDto.Category;
				product.Stock = productDto.Stock;
				await _productRepository.UpdateProductAsync(product);
			}
		}

		public async Task DeleteProductAsync(int id)
		{
			await _productRepository.DeleteProductAsync(id);
		}
	}
}
