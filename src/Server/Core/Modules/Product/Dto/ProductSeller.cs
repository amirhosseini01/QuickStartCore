using System.ComponentModel;
using Server.Core.Commons;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Dto;

public class ProductSellerFilter
{
    [DisplayName(Messages.User)]
    public int? UserId { get; set; }
}


public class ProductSellerInput: ProductSeller
{
    [DisplayName(Messages.Logo)]
    public IFormFile? LogoFile { get; set; }
}
