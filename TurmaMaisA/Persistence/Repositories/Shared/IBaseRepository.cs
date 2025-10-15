using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace TurmaMaisA.Persistence.Repositories.Shared
{
    public interface IBaseRepository<TEntity>
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
