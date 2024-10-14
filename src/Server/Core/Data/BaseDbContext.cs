using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Notification.Models;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Sale.Models;
using Server.Core.Modules.User.Models;

namespace Server.Core.Data;

public class BaseDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
	//  dotnet ef --startup-project Server migrations add Init --project Server --context BaseDbContext
	//  dotnet ef --startup-project Server database update --project Server --context BaseDbContext
	public BaseDbContext(DbContextOptions options) : base(options) { }

	// products
	public DbSet<Product> Products { get; set; }
	public DbSet<ProductBrand> ProductBrands { get; set; }
	public DbSet<ProductCategory> ProductCategories { get; set; }
	public DbSet<ProductComment> ProductComments { get; set; }
	public DbSet<ProductModel> ProductModels { get; set; }
	public DbSet<ProductSeller> ProductSellers { get; set; }
	public DbSet<ProductStock> ProductStocks { get; set; }

	// CMS
	public DbSet<Slider> Sliders { get; set; }
	public DbSet<NewsCategory> NewsCategories { get; set; }
	public DbSet<News> News { get; set; }

	// Sales
	public DbSet<Order> Orders { get; set; }
	public DbSet<OrderDetail> OrderDetails { get; set; }

	// User
	public DbSet<UserAddress> UserAddresses { get; set; }

	// Notification
	public DbSet<NotificationLog> NotificationLogs { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}
