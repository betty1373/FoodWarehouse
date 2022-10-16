using FW.BusinessLogic.Contracts.Dishes;
using FW.ResponseStatus;
namespace FW.BusinessLogic.Services.Abstractions;

public interface IDishesService
{
    /// <summary>
    /// Получить список изменений продуктов
    /// </summary>
    /// <returns> список изменений продуктов </returns>
    public Task<IEnumerable<DishResponseDto>> GetAll();

    /// <summary>
    /// Получить количество всех категорий
    /// </summary>
    /// <returns> количество всех категорий </returns>
    public Task<int> Count();
    /// <summary>
    /// Получить список блюд
    /// </summary>
    /// <param name="pageNumber">номер страницы</param>
    /// <param name="pageSize">объем страницы</param>
    /// <returns> список блюд </returns>
    public Task<IEnumerable<DishResponseDto>> GetPaged(int Skip, int Take);

    /// <summary>
    /// Получить блюдо по Id
    /// </summary>
    /// <param name="id"> идентификатор </param>
    /// <returns> Id блюда </returns>
    public Task<DishResponseDto> GetById(Guid id);

    /// <summary>
    /// Создать блюдо
    /// </summary>
    /// <param name="dto">DTO создания блюда</param>
    /// <returns> идентификатор нового блюда </returns>
    public Task<Guid> Create(DishCreateDto dto);

    /// <summary>
    /// Удалить блюдо
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <returns>true: удален, false: не найден </returns>
    public Task<bool> Delete(Guid id);

    /// <summary>
    /// Изменить блюдо
    /// </summary>
    /// <param name="dto">DTO обновления блюда</param>
    /// <returns>true: обновлен, false: не найден </returns>
    public Task<bool> Update(DishUpdateDto dto);
    public Task<IEnumerable<DishResponseDto>> GetByParentId(Guid ParentId);
    public Task<ResponseStatusResult> Cook(Guid dishId, Guid warehauseId, int numPortions);
}
