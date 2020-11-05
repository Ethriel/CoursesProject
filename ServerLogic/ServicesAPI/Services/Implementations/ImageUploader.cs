using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServicesAPI.Helpers;
using ServicesAPI.Services.Abstractions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ServicesAPI.Services.Implementations
{
    public class ImageUploader : IImageUploader
    {
        private readonly IServerService serverService;

        public ImageUploader(IServerService serverService)
        {
            this.serverService = serverService;
        }

        public string UploadImage(IFormFile image, string folder, int maxWidth = 400, int maxHeight = 400)
        {
            var imagePath = SaveImage(image, folder, maxWidth, maxHeight);

            return imagePath;
        }

        public string SaveImage(IFormFile image, string folder, int maxWidth = 400, int maxHeight = 400)
        {
            var filename = Guid.NewGuid().ToString();

            var ext = Path.GetExtension(image.FileName);

            var root = serverService.GetRootPath(folder);

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
                    var savedImage = CreateImage(bmp, maxWidth, maxHeight);
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

        private Bitmap CreateImage(Bitmap originalPic, int maxWidth, int maxHeight)
        {
            try
            {
                var width = originalPic.Width;
                var height = originalPic.Height;
                var widthDiff = width - maxWidth;
                var heightDiff = height - maxHeight;
                var doWidthResize = (maxWidth > 0 && width > maxWidth && widthDiff > heightDiff);
                var doHeightResize = (maxHeight > 0 && height > maxHeight && heightDiff > widthDiff);

                if (doWidthResize || doHeightResize || (width.Equals(height) && widthDiff.Equals(heightDiff)))
                {
                    var iStart = default(int);
                    var divider = default(decimal);
                    if (doWidthResize)
                    {
                        iStart = width;
                        divider = Math.Abs((decimal)iStart / maxWidth);
                        width = maxWidth;
                        height = (int)Math.Round((height / divider));
                    }
                    else
                    {
                        iStart = height;
                        divider = Math.Abs((decimal)iStart / maxHeight);
                        height = maxHeight;
                        width = (int)Math.Round(width / divider);
                    }
                }
                using (var outBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics oGraphics = Graphics.FromImage(outBmp))
                    {
                        oGraphics.DrawImage(originalPic, 0, 0, width, height);
                        //Водяний знак
                        //Font font = new Font("Arial", 20);
                        //Brush brash = new SolidBrush(Color.Blue);
                        //oGraphics.DrawString("Hello Vova", font, brash, new Point(25, 25));
                        return new Bitmap(outBmp);
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
