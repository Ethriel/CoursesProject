using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface ICoursesService
    {
        Task<ApiResult> GetAmountAsync();
        Task<ApiResult> GetAllCoursesAsync();
        Task<ApiResult> GetForPage(int skip, int take);
        Task<ApiResult> GetById(int id);
    }
}
