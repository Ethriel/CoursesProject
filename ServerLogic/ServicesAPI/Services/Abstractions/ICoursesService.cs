using ServicesAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface ICoursesService
    {
        Task<int> GetAmountAsync();
        Task<IEnumerable<TrainingCourseDTO>> GetAllCoursesAsync();
        Task<IEnumerable<TrainingCourseDTO>> GetForPage(int skip, int take);
        Task<TrainingCourseDTO> GetById(int id);
    }
}
