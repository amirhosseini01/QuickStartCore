using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Server.Test;

public class MockIFormFile
{
    public static IFormFile CreateMockFormFile(string fileName, string contentType, string content)
    {
        // Create a mock file stream
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        // Substitute IFormFile
        var formFile = Substitute.For<IFormFile>();
        formFile.FileName.Returns(fileName);
        formFile.ContentType.Returns(contentType);
        formFile.OpenReadStream().Returns(stream);
        formFile.Length.Returns(stream.Length);

        return formFile;
    }
}