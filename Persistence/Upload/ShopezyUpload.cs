using Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Persistence.Upload;

public class  ShopezyUpload : IShopezyUpload
{



    private readonly IWebHostEnvironment _webHostEnvironment;
    public ShopezyUpload(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<string> UploadFiles(IFormFile formFile)
    {
         var pathName = Path.Combine(_webHostEnvironment.WebRootPath,"assets/images");
            var filename = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            if (!Directory.Exists(pathName))
            {
                Directory.CreateDirectory(pathName);
            }
            var absolutePath = Path.Combine(pathName, filename);
            await formFile.CopyToAsync(new FileStream(absolutePath, FileMode.Create));
            return filename;
    }

}
