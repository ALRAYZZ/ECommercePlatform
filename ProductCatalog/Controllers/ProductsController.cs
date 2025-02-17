using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Services;

namespace ProductCatalog.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService=productService;
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null)
		{
			var products = await _productService.GetAllProductsAsync(page, pageSize, category);
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetProductById(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
		{
			await _productService.AddProductAsync(productDto);
			return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
		{
			if (id != productDto.Id)
			{
				return BadRequest();
			}
			await _productService.UpdateProductAsync(productDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			await _productService.DeleteProductAsync(id);
			return NoContent();
		}
	}
}
