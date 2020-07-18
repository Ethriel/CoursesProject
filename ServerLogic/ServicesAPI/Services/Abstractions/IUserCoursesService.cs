using ServicesAPI.DTO;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IUserCoursesService
    {
        Task<ApiResult> AddCourseToUserAsync(SystemUsersTrainingCoursesDTO userCourseDTO);
        Task<ApiResult> GetAllAsync();
        Task<ApiResult> GetAmountAsync();
        Task<ApiResult> GetForPageAsync(int skip, int take);
        Task<ApiResult> GetByUserIdAsync(int id);
    }
}
