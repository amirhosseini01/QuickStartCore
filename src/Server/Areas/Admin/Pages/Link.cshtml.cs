using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;

namespace Server.Areas.Admin.Pages;

public class Link : PageModel
{
    public JsonResult OnGetEditorPreview(string url)
    {
        return ResponseBase.ReturnJson(new
        {
            success = 1,
            link = url,
            meta = new
            {
                
            }
        });
    }
}