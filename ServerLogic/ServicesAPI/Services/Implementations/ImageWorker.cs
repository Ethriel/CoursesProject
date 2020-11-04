using ServicesAPI.DTO;
using ServicesAPI.Services.Abstractions;
using System.Collections.Generic;
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

        public string CutImageToName(string image)
        {
            if (!image.Contains("/share/img/"))
            {
                return image;
            }
            var newImage = Path.GetFileName(image);
            return newImage;
        }

        public void DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string GetImageRootPath(string folder, string image)
        {
            var root = serverService.GetRootPath(folder);
            var path = Path.Combine(root, image);
            return path;
        }

        public string GetImageURL(string folder, string image)
        {
            if (string.IsNullOrWhiteSpace(image))
            {
                return null;
            }

            if (image.Contains("http"))
            {
                return image;
            }

            var serverRoot = serverService.GetServerURL();
            var imagePath = string.Concat("share/", "img/", folder, "/", image);
            var avatarPath = string.Concat(serverRoot, imagePath);
            return avatarPath;
        }

        public void SetCoursesImages(IEnumerable<TrainingCourseDTO> courses)
        {
            foreach (var course in courses)
            {
                if (!course.Cover.Contains("http"))
                {
                    course.Cover = GetImageURL("courses", course.Cover);
                }
            }
        }

        public void SetUsersImages(IEnumerable<SystemUserDTO> users)
        {
            foreach (var user in users)
            {
                if (!user.AvatarPath.Contains("http"))
                {
                    user.AvatarPath = GetImageURL("courses", user.AvatarPath);
                }
            }
        }
    }
}
