using Microsoft.AspNetCore.Http;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface ICoursesService
    {
        Task<ApiResult> GetAllCoursesAsync();
        Task<ApiResult> GetByIdAsync(int id);
        Task<ApiResult> GetPagedAsync(CoursesPagination coursesPagination);
        Task<ApiResult> CheckCourseAsync(int userId, int courseId);
        Task<ApiResult> CreateCourseAsync(TrainingCourseDTO courseDTO);
        Task<ApiResult> UpdateCourseAsync(TrainingCourseDTO courseDTO);
        Task<ApiResult> UploadImageAsync(IFormFile image, int id);
    }
}
