using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
