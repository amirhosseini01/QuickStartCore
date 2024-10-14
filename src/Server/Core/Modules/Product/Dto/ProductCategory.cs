using System.ComponentModel;
using Server.Core.Commons;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Dto;

public class ProductCategoryFilter
{
    [DisplayName(Messages.Visible)]
    public bool? Visible { get; set; }
}

public class ProductCategoryInput: ProductCategory
{
    [DisplayName(Messages.Image)]
    public IFormFile? ImageFile { get; set; }
}
