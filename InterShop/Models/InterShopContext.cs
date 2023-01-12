using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace InterShop.Models
{
	public class InterShopContext : DbContext
	{
		public DbSet<User> users{get;set;}
		public DbSet<Product> products { get; set; }
		public DbSet<Category> categories { get; set; }
		public DbSet<Order> orders { get; set; }
		public DbSet<OrderState> orderStates { get; set; }
		public DbSet<Photo> photos { get; set; }
		public DbSet<ProductOrder> productOrders { get; set; }
		public DbSet<Role> roles { get; set; }

		public InterShopContext(DbContextOptions<InterShopContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
    }
}
