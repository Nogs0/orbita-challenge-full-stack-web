using System.Linq.Expressions;

namespace TurmaMaisA.Persistence.Repositories.Shared
{
    public interface IBaseRepository<TEntity>
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<PagedResult<TEntity>> GetPagedItemsAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? predicate = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<int> CountWithIgnoreQueryFiltersAsync(Expression<Func<TEntity, bool>>? predicate = null);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
