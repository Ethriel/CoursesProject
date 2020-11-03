using Microsoft.AspNetCore.Http;
using ServicesAPI.Services.Abstractions;
using System.IO;

namespace ServicesAPI.Services.Implementations
{
    public class ImageWorker : IImageWorker
    {
        private readonly IServerService serverService;

        public ImageWorker(IImageUploader imageUploader, IServerService serverService)
        {
            ImageUploader = imageUploader;
            this.serverService = serverService;
        }

        public IImageUploader ImageUploader { get; }

        public string GetImageRootPath(string folder, string image)
        {
            var root = serverService.GetRootPath("users");
            var path = Path.Combine(root, image);
            return path;
        }

        public string GetImageURL(string folder, string image, HttpContext httpContext)
        {
            if (string.IsNullOrWhiteSpace(image))
            {
                return null;
            }
            var serverRoot = serverService.GetServerURL(httpContext);
            var imagePath = string.Concat("share/", "img/", folder, "/", image);
            var avatarPath = string.Concat(serverRoot, imagePath);
            return avatarPath;
        }
    }
}
