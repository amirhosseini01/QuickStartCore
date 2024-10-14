namespace Server.Core.Commons.UploadFile;

public class FileUploaderException : Exception
{
    public FileUploaderException()
    {
    }

    public FileUploaderException(string message) : base(message)
    {
    }
}
