using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.Product.Extensions;
using Server.Core.Modules.Sale.Models;

namespace Server.Core.Modules.Product.Models;

public class Product : BaseEntity
{
    [DisplayName(Messages.Category)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int ProductCategoryId { get; set; }

    [DisplayName(Messages.Brand)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int ProductBrandId { get; set; }

    [DisplayName(Messages.Seller)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int ProductSellerId { get; set; }

    [DisplayName(Messages.ViewOrder)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int? ViewOrder { get; set; }

    public int ViewCount { get; set; }

    [DisplayName(Messages.Title)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public required string Title { get; set; }

    [DisplayName(Messages.Image)]
    [StringLength(ModelStatics.ImageRequiredLength)]
    public string? Image { get; set; }

    [DisplayName(Messages.Thumbnail)]
    [StringLength(ModelStatics.ImageRequiredLength)]
    public string? Thumbnail { get; set; }

    [DisplayName(Messages.Visible)]
    public bool Visible { get; set; } = true;

    [DisplayName(Messages.Saleable)]
    public bool Saleable { get; set; } = true;

    [DisplayName(Messages.IsSpecialOffer)]
    public bool IsSpecialOffer { get; set; }

    [DisplayName(Messages.ShortDescription)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.DescriptionRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.DescriptionMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public required string ShortDescription { get; set; }

    [DisplayName(Messages.Description)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MinLength(ModelStatics.DescriptionMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public required string Description { get; set; }

    [NotMapped]
    public decimal? BestPrice => ProductModels?.ToList().GetBestPrice();
    
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required ProductSeller? ProductSeller { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required ProductCategory? ProductCategory { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required ProductBrand? ProductBrand { get; set; }
    public required ICollection<ProductComment>? ProductComments { get; set; }
    public required ICollection<ProductModel>? ProductModels { get; set; }
    public required ICollection<ProductStock>? ProductStocks { get; set; }
    public required ICollection<OrderDetail>? OrderDetails { get; set; }
}
