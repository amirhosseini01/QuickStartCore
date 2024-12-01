using FileSignatures;
using FileSignatures.Formats;

namespace Server.Core.Commons.UploadFile;

public interface IFileUploader
{
    Task<ResponseDto<string>> UploadFile(IFormFile? file);
    void DeleteFile(string fileUrl);
}
public class FileUploader: IFileUploader
{
    private readonly string[] _permittedExtensions = { ".PNG", ".JPG", ".JPEG", ".WEBP" };
    private readonly UploadFileOptions _options;
    private readonly IFileFormatInspector _inspector;
    public FileUploader(IConfiguration configuration,
     IFileFormatInspector inspector)
    {
        _options = new UploadFileOptions();
        configuration.GetSection(UploadFileOptions.UploadFile).Bind(_options);
        _inspector = inspector;
    }
    public async Task<ResponseDto<string>> UploadFile(IFormFile? file)
    {
        if (file is null || file.Length <= 0)
        {
            return ResponseBase.Failed<string>(Messages.SelectFile);
        }

        if (string.IsNullOrEmpty(_options.StoredImagesFolder) || string.IsNullOrEmpty(_options.StoredImagesPath))
        {
            return ResponseBase.Failed<string>(Messages.EnterStoredFilesPath);
        }

        if (file.Length > _options.FileSizeLimit)
        {
            return ResponseBase.Failed<string>(Messages.EnterStoredFilesPath);
        }

        var validationRes = ValidateFileExtension(file, _permittedExtensions);
        if (validationRes.IsFailed)
        {
            return validationRes;
        }

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var storedFilesPath = _options.StoredImagesPath;
        var absolutePath = Path.Combine(storedFilesPath, fileName);
        var relativePath = $"/{_options.StoredImagesFolder}/{fileName}";

        await using (var stream = file.OpenReadStream())
        {
            validationRes = ValidateSignature(stream);
            if (validationRes.IsFailed)
            {
                return validationRes;
            }
        }

        await using (var stream = File.Create(absolutePath))
        {
            await file.CopyToAsync(stream);
        }

        return ResponseBase.Success(obj: relativePath);
    }

    public void DeleteFile(string fileUrl)
    {
        File.Delete($@"{Directory.GetCurrentDirectory()}\wwwroot\{fileUrl}");
    }

    private static ResponseDto<string> ValidateFileExtension(IFormFile file, string[] permittedExtensions)
    {
        var ext = Path.GetExtension(file.FileName).ToUpperInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            return ResponseBase.Failed<string>(Messages.FileExtensionNotAllowed);
        }

        return ResponseBase.Success<string>();
    }

    private ResponseDto<string> ValidateSignature(Stream stream)
    {
        var format = _inspector.DetermineFileFormat(stream);

        if (format is Pdf)
        {
            return ResponseBase.Success<string>();
        }

        if (format is Png)
        {
            return ResponseBase.Success<string>();
        }

        if (format is Image)
        {
            return ResponseBase.Success<string>();
        }

        if (format is Webp)
        {
            return ResponseBase.Success<string>();
        }

        if (format is Jpeg)
        {
            return ResponseBase.Success<string>();
        }

        return ResponseBase.Failed<string>(Messages.FileNotAllowed);
    }
}
