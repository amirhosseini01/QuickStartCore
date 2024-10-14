using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;
using Server.Core.Data;

namespace Server.Core.Modules.Product.Models;

public class ProductCategory : BaseEntity
{
    [DisplayName(Messages.ViewOrder)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int? ViewOrder { get; set; }
    
    [DisplayName(Messages.Title)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public string Title { get; set; }
    
    [DisplayName(Messages.Visible)] 
    public bool Visible { get; set; }

    [DisplayName(Messages.Image)] 
    [StringLength(ModelStatics.ImageRequiredLength)]
    public string? Image { get; set; }

    public ICollection<Product>? Products { get; set; }
}
