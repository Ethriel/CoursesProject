using ServicesAPI.DataPresentation;
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
    }
}
