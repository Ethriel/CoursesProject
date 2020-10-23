using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface ICoursesService
    {
        Task<ApiResult> GetAllCoursesAsync();
        Task<ApiResult> GetById(int id);
        Task<ApiResult> GetPagedAsync(CoursesPagination coursesPagination);
        Task<ApiResult> CheckCourseAsync(int userId, int courseId);
        Task<ApiResult> CreateCourseAsync(TrainingCourseDTO courseDTO);
        Task<ApiResult> UpdateCourseAsync(TrainingCourseDTO courseDTO);
    }
}
