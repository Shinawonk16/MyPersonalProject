using Microsoft.AspNetCore.Http;

namespace Application.Abstractions;

public interface IShopezyUpload
{
    Task<string> UploadFiles(IFormFile formFile);
}
