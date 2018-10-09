using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.SharedKernel.Interfaces
{
    public interface IRepository<T,TId>
    {
        Task<List<T>> GetAllAsync();
        Task CreateOrUpdateAsync(T entity);
        Task RemoveAsync(TId id);
        Task SaveAsync();
    }
}