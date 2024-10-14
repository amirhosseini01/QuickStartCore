namespace Server.Core.Commons.UploadFile;

public class UploadFileOptions
{
    public const string UploadFile = "UploadFile";

    public string StoredImagesFolder { get; set; } = string.Empty;
    public string StoredImagesPath { get; set; } = string.Empty;
    public long FileSizeLimit { get; set; }
}
