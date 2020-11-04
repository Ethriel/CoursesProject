using Microsoft.AspNetCore.Http;

namespace ServicesAPI.Services.Abstractions
{
    public interface IImageUploader
    {
        string UploadImage(IFormFile image, string folder, int maxWidth = 400, int maxHeight = 400);
        string GetPathForURL(string image, string folder);
        string SaveImage(IFormFile image, string folder, int maxWidth = 400, int maxHeight = 400);
    }
}
