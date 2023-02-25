using VideoGameStore.Models;
using System.Linq.Expressions;

namespace VideoGameStore.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        // It is a good practice to use a service to access the database instead of directly
        // accessing it from a controller. Thus we create the following methods below 
        // That can be called from the controller
        //Get all actors from Database, make it a "task" to allow it to be an async enumerable
        Task<IEnumerable<T>> GetAllAsync();
        // Overload
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(T deleteEntity);

        Task<T> SearchAsync(T entity);
    }
}
