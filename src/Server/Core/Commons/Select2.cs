using Microsoft.AspNetCore.Mvc.Rendering;

namespace Server.Core.Commons;

public class Select2
{

}

public class Select2Results
{
    public bool HasNextPage { get; set; }
    public List<SelectListItem> Items { get; set; }
}