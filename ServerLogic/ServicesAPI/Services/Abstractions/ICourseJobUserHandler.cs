using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface ICourseJobUserHandler
    {
        Task AddAsync(string jobId, int courseId, int userId);
        Task<ApiResult> DeleteAsync(int courseId, int userId);
    }
}
