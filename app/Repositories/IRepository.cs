using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Repositories;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}