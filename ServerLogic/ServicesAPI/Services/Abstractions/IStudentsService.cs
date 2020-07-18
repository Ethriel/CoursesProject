using ServicesAPI.Responses;
using ServicesAPI.Sorts;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IStudentsService
    {
        Task<ApiResult> GetAllStudentsAsync();
        Task<ApiResult> GetAmountOfStudentsAync();
        Task<ApiResult> GetSortedStudentsAsync(Sorting sorting);
        Task<ApiResult> SearchStudentsAsync(string search);
    }
}
