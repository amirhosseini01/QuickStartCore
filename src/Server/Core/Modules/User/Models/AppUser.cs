using Microsoft.AspNetCore.Identity;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Sale.Models;

namespace Server.Core.Modules.User.Models;

// This is our ApiUser, we can modify this if we need
// to add custom properties to the user
public class AppUser : IdentityUser<int>
{
    public ICollection<ProductSeller>? ProductSellers { get; set; }
    public ICollection<ProductComment>? ProductComments { get; set; }
    public ICollection<ProductStock>? ProductStocks { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
