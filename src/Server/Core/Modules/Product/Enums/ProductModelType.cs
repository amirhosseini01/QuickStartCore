using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;

namespace Server.Core.Modules.Product.Enums;

public enum ProductModelType
{
	[Display(Name = Messages.Color)]
    Color,
	[Display(Name = Messages.Size)]
    Size
}
