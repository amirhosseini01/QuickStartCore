using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.Product.Models;

public class ProductComment : BaseEntity
{
    [DisplayName(Messages.User)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int UserId { get; set; }
    
    [DisplayName(Messages.Product)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int ProductId { get; set; }
    
    public bool Approved { get; set; }

    [StringLength(ModelStatics.DescriptionRequiredLength)]
    public string Comment { get; set; }
    public DateTime CreateDate { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Product? Product { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public AppUser? User { get; set; }
}
