using Microsoft.AspNetCore.Http;

namespace ServicesAPI.Services.Abstractions
{
    public interface IImageUploader
    {
        string UploadImage(IFormFile image, string folder);
        string GetPathForURL(string image, string folder);
    }
}
