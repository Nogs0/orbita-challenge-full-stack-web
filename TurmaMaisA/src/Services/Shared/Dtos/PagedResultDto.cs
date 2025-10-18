namespace TurmaMaisA.Services.Shared.Dtos
{
    public class PagedResultDto<TListDto>
    {
        public List<TListDto> Items { get; set; } = [];
        public long TotalCount { get; set; }

        public PagedResultDto(List<TListDto> items, long totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}
