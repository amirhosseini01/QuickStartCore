using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.EditorJs.Dtos;
using Server.Core.Commons.UploadFile;

namespace Server.Areas.Admin.Pages;

public class FileUploaderModel(
	FileUploader fileUploader
	) : PageModel
{
	public async Task<JsonResult> OnPostUploadFile(EditorJsUploadFileInputDto input)
	{
		var result = await fileUploader.UploadFile(file: input.File);
		return ResponseBase.ReturnJson(result);
	}
}
