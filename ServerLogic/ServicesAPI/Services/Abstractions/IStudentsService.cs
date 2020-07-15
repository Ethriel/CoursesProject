using ServicesAPI.DTO;
using ServicesAPI.Sorts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IStudentsService
    {
        Task<IEnumerable<SystemUserDTO>> GetAllStudentsAsync();
        Task<int> GetAmountOfStudentsAync();
        Task<IEnumerable<SystemUserDTO>> GetSortedStudentsAsync(Sorting sorting);
    }
}
