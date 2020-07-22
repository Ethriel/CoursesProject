using ServicesAPI.Responses;
using ServicesAPI.DataPresentation;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IStudentsService
    {
        Task<ApiResult> GetAllStudentsAsync();
        Task<ApiResult> GetUserByIdAsync(int id);
        Task<ApiResult> SearchAndSortStudentsAsync(SearchAndSort searchAndSort);
    }
}
