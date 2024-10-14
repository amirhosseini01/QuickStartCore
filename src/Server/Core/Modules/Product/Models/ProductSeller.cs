using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.Product.Models;

public class ProductSeller : BaseEntity
{
    [DisplayName(Messages.User)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int UserId { get; set; }

    [DisplayName(Messages.Title)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public string Title { get; set; }

    [DisplayName(Messages.PhoneNumber)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MinLengthModelState)]
    public string PhoneNumber { get; set; }

    [DisplayName(Messages.Logo)]
    public string? Logo { get; set; }

    [DisplayName(Messages.PostalAddress)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public string? PostalAddress { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public AppUser? User { get; set; }
}
