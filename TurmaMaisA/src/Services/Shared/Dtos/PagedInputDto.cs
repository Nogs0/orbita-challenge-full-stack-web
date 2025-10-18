namespace TurmaMaisA.Services.Shared.Dtos
{
    public class PagedInputDto
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 0;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Search { get; set; }
    }
}
