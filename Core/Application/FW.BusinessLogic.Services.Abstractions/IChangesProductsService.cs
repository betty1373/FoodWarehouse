using FW.BusinessLogic.Contracts.ChangesProducts;
using FW.ResponseStatus;

namespace FW.BusinessLogic.Services.Abstractions;

public interface IChangesProductsService
{
    /// <summary>
    /// Получить список изменений продуктов
    /// </summary>
    /// <returns> список изменений продуктов </returns>
    public Task<IEnumerable<ChangesProductResponseDto>> GetAll();

    /// <summary>
    /// Получить количество всех категорий
    /// </summary>
    /// <returns> количество всех категорий </returns>
    public Task<int> Count();

    /// <summary>
    /// Получить список изменений продуктов
    /// </summary>
    /// <param name="pageNumber">номер страницы</param>
    /// <param name="pageSize">объем страницы</param>
    /// <returns> список изменений продуктов </returns>
    public Task<IEnumerable<ChangesProductResponseDto>> GetPaged(int Skip, int Take);

    /// <summary>
    /// Получить изменение продукта по Id
    /// </summary>
    /// <param name="id"> идентификатор </param>
    /// <returns> Id изменение продукта </returns>
    public Task<ChangesProductResponseDto> GetById(Guid id);
    /// <summary>
    /// Получить изменения по продукту 
    /// </summary>
    /// <param name="id"> идентификатор </param>
    /// <returns> Id рецепта </returns>
    public Task<IEnumerable<ChangesProductResponseDto>> GetByParentId(Guid ParentId);
    /// <summary>
    /// Создать изменение продукта
    /// </summary>
    /// <param name="dto">DTO создания изменения продукта</param>
    /// <returns> идентификатор нового изменения продукта </returns>
    public Task<Guid> Create(ChangesProductCreateDto dto);

    /// <summary>
    /// Удалить изменение продукта
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <returns>true: удален, false: не найден </returns>
    public Task<bool> Delete(Guid id);

    /// <summary>
    /// Изменить изменение продукта
    /// </summary>
    /// <param name="dto">DTO обновления изменения продукта</param>
    /// <returns>true: обновлен, false: не найден </returns>
    public Task<bool> Update(ChangesProductUpdateDto dto);
}
