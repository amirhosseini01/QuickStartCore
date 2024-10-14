using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.Sale.Enums;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.Sale.Models;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public int? UserAddressId { get; set; }

    [Column(TypeName = ModelStatics.Nvarchar50)]
    public OrderStatus OrderStatus { get; set; }

    [StringLength(ModelStatics.DescriptionRequiredLength)]
    public string? AdminNote { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public AppUser User { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public UserAddress UserAddress { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; }
}
