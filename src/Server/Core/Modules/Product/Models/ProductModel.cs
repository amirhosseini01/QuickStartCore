using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Commons.Extensions;
using Server.Core.Data;
using Server.Core.Modules.Product.Enums;
using Server.Core.Modules.Product.Extensions;
using Server.Core.Modules.Sale.Models;

namespace Server.Core.Modules.Product.Models;

public class ProductModel : BaseEntity
{
    [DisplayName(Messages.Product)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int ProductId { get; set; }

    [DisplayName(Messages.ViewOrder)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int? ViewOrder { get; set; }

    [DisplayName(Messages.Title)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public string Title { get; set; }

    [DisplayName(Messages.Type)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Column(TypeName = ModelStatics.Nvarchar50)]
    public ProductModelType Type { get; set; }

    [DisplayName(Messages.Value)]
    [Required(ErrorMessage = Messages.Value)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public string Value { get; set; }
    
    [DisplayName(Messages.Price)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    public decimal Price { get; set; }
    
    [DisplayName(Messages.Discount)]
    public decimal? Discount { get; set; }
    
    [NotMapped]
    public decimal FinalPrice => ProductHelper.GetFinalPrice(price: Price, discount: Discount);
    
    [NotMapped]
    public string TypeStr => Type.GetEnumDisplayName();

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Product? Product { get; set; }
    public ICollection<ProductStock>? ProductStocks { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}
