namespace TurmaMaisA.Persistence.Repositories.Shared
{
    public interface IBaseRepository<TEntity>
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
