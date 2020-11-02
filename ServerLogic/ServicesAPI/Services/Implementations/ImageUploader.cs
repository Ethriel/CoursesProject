using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServicesAPI.Helpers;
using ServicesAPI.Services.Abstractions;
using System;
using System.Drawing;
using System.IO;

namespace ServicesAPI.Services.Implementations
{
    public class ImageUploader : IImageUploader
    {
        private readonly IWebHostEnvironment webHost;

        public ImageUploader(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        public string UploadImage(IFormFile image, string folder)
        {
            var imagePath = SaveImage(image, folder);

            return imagePath;
        }

        private string SaveImage(IFormFile image, string folder)
        {
            var filename = Guid.NewGuid().ToString();

            var ext = Path.GetExtension(image.FileName);

            var root = Path.Combine(webHost.WebRootPath, "share", "img", folder);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            filename = string.Concat(filename, ext);

            var fullPath = Path.Combine(root, filename);

            using (var stream = image.OpenReadStream())
            {
                using (var bmp = new Bitmap(stream))
                {
                    var savedImage = ImageHelper.CreateImage(bmp, 400, 400);
                    savedImage.Save(fullPath);
                }
            }

            return filename;
        }

        public string GetPathForURL(string image, string folder)
        {
            var path = string.Concat("share/", "img/", folder, "/", image);
            return path;
        }
    }
}
