using ServicesAPI.DTO;
using System.Collections.Generic;

namespace ServicesAPI.Services.Abstractions
{
    public interface IImageWorker
    {
        IImageUploader ImageUploader { get; }
        string GetImageURL(string folder, string image);
        string GetImageRootPath(string folder, string image);
        void DeleteImage(string path);
        void SetCoursesImages(IEnumerable<TrainingCourseDTO> courses);
        void SetUsersImages(IEnumerable<SystemUserDTO> users);
        string CutImageToName(string image);
    }
}
