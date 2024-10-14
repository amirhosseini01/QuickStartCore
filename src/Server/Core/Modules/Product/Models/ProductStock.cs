using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.Product.Models;

public class ProductStock : BaseEntity
{
    [DisplayName(Messages.User)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int? UserId { get; set; }
    
    [DisplayName(Messages.Product)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int ProductId { get; set; }
    
    [DisplayName(Messages.ProductModel)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange)]
    public int ProductModelId { get; set; }
    
    [DisplayName(Messages.IsReserved)]
    public bool IsReserved { get; set; }
    
    [DisplayName(Messages.Value)]
    [Required(ErrorMessage = Messages.RegisterConfirmation)]
    public int Value { get; set; }
    
    public DateTime CreateDate { get; set; } = DateTime.Now; 

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Product? Product { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public ProductModel? ProductModel { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public AppUser? User { get; set; }
}
