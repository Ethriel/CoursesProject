using Microsoft.AspNetCore.Http;
using ServicesAPI.DataPresentation;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IStudentsService
    {
        Task<ApiResult> GetAllStudentsAsync();
        Task<ApiResult> GetUserByIdAsync(int id);
        Task<ApiResult> SearchAndSortStudentsAsync(SearchAndSort searchAndSort);
        Task<ApiResult> UploadImageAsync(IFormFile image, int id);
    }
}
