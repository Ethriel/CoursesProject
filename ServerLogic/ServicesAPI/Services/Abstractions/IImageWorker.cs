using Microsoft.AspNetCore.Http;

namespace ServicesAPI.Services.Abstractions
{
    public interface IImageWorker
    {
        IImageUploader ImageUploader { get; }
        string GetImageURL(string folder, string image, HttpContext httpContext);
        string GetImageRootPath(string folder, string image);
    }
}
