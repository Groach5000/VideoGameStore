using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace VideoGameStore.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _context;

        public EntityBaseRepository(AppDbContext context)
        {
            _context = context;
        }

         ~EntityBaseRepository()
        {
            _context.Dispose();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T deleteEntity)
        {
            EntityEntry entityEntry = _context.Entry<T>(deleteEntity);
            entityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }
        // Note that the SaveChangesAsync is not required, so the code could be simplified to the following:
        // public async Task DeleteAsync(T deleteEntity) => _context.Set<T>().Remove(deleteEntity);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Old code below that was in Publister Service, now using "Set<T>()" instead of Publister since we can
            // Take multiple models and perform this same action.
            //var result = await _context.Publister.ToListAsync();
            return await _context.Set<T>().ToListAsync();
        }
        // Above could be simplified to: public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperties) => current.Include(includeProperties));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperties) => current.Include(includeProperties));
            return await query.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<T> SearchAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAllAsQueriable()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}
