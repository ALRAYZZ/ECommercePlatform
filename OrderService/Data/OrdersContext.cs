using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
	public class OrdersContext : DbContext
	{
		public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
		{
		}
		public DbSet<OrdersModel> Orders { get; set; }	
	}
}
