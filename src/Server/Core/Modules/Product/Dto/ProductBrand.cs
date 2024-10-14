using System.ComponentModel;
using Server.Core.Commons;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Dto;

public class ProductBrandFilter
{
    [DisplayName(Messages.Visible)]
    public bool? Visible { get; set; }
}

public class ProductBrandInput: ProductBrand
{
    [DisplayName(Messages.Logo)]
    public IFormFile? LogoFile { get; set; }
}
