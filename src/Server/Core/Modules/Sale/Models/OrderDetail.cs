using Microsoft.EntityFrameworkCore;
using Server.Core.Data;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Sale.Models;

public class OrderDetail : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int ProductModelId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Order Order { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Product.Models.Product Products { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public ProductModel ProductModel { get; set; }
}
