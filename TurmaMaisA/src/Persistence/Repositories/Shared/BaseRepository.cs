
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TurmaMaisA.Models.Shared;
using TurmaMaisA.Persistence;

namespace TurmaMaisA.Persistence.Repositories.Shared
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        protected BaseRepository(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.ToListAsync();

            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.AnyAsync();

            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountWithIgnoreQueryFiltersAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.IgnoreQueryFilters().CountAsync();

            return await _dbSet.IgnoreQueryFilters().CountAsync(predicate);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<PagedResult<TEntity>> GetPagedItemsAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = _dbSet.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            var skipAmount = (pageNumber - 1) * pageSize;

            var totalCount = await query.CountAsync();
            query = query.Skip(skipAmount).Take(pageSize);

            return new PagedResult<TEntity>(await query.ToListAsync(), totalCount);
        }
    }
}
