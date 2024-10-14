using System.ComponentModel;
using Server.Core.Commons;

namespace Server.Core.Modules.Product.Dto;

public class ProductModelFilter
{
    [DisplayName(Messages.Product)]
    public int? ProductId { get; set; }
}