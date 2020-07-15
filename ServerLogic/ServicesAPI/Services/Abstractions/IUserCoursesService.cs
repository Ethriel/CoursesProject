using ServicesAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IUserCoursesService
    {
        Task AddCourseToUserASync(SystemUsersTrainingCoursesDTO userCourseDTO);
        Task<IEnumerable<SystemUsersTrainingCoursesDTO>> GetAllAsync();
        Task<int> GetAmountAsync();
        Task<IEnumerable<SystemUsersTrainingCoursesDTO>> GetForPageAsync(int skip, int take);
        Task<IEnumerable<SystemUsersTrainingCoursesDTO>> GetByUserIdAsync(int id);
    }
}
