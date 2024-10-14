using System.ComponentModel;
using Server.Core.Commons;

namespace Server.Core.Modules.Product.Dto;

public class ProductStockFilter
{
    [DisplayName(Messages.Product)]
    public int? ProductId { get; set; }
    
    [DisplayName(Messages.ProductModel)]
    public int? ProductModelId { get; set; }
}