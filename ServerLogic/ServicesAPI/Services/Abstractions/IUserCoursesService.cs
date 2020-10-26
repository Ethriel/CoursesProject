using ServicesAPI.DTO;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IUserCoursesService
    {
        Task<ApiResult> AddCourseToUserAsync(SystemUsersTrainingCoursesDTO userCourseDTO);
        Task<ApiResult> GetAllAsync();
        Task<ApiResult> GetByUserIdAsync(int id);
        Task<ApiResult> Unsubscribe(int userId, int courseId);
    }
}
