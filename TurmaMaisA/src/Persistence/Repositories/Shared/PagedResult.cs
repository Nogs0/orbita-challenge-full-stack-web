namespace TurmaMaisA.Persistence.Repositories.Shared
{
    public class PagedResult<TEntity>
    {
        public List<TEntity> Items { get; set; } = [];
        public long TotalCount { get; set; }

        public PagedResult(List<TEntity> items, long totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}
