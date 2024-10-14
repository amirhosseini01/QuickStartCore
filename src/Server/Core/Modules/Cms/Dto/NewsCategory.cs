using System.ComponentModel;
using Server.Core.Commons;

namespace Server.Core.Modules.Cms.Dto;

public class NewsCategoryFilter
{
    [DisplayName(Messages.Visible)]
    public bool? Visible { get; set; }
}
