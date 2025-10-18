using TurmaMaisA.Services.Shared.Dtos;

namespace TurmaMaisA.Services.Shared
{
    public interface IBaseServiceCrud<TEntity, TEntityDto, TListDto, TCreateDto, TUpdateDto>
    {
        Task<TEntityDto> CreateAsync(TCreateDto dto);

        Task<TEntityDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TListDto>> GetAllAsync();
        Task UpdateAsync(TUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
