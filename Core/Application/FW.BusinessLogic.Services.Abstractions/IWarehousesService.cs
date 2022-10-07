using FW.BusinessLogic.Contracts.Warehouses;

namespace FW.BusinessLogic.Services.Abstractions;

public interface IWarehousesService
{
    /// <summary>
    /// Получить список изменений продуктов
    /// </summary>
    /// <returns> список изменений продуктов </returns>
    public Task<IEnumerable<WarehouseResponseDto>> GetAll();
    /// <summary>
    /// Получить количество всех категорий
    /// </summary>
    /// <returns> количество всех категорий </returns>
    public Task<int> Count();
    /// <summary>
    /// Получить список складов
    /// </summary>
    /// <param name="pageNumber">номер страницы</param>
    /// <param name="pageSize">объем страницы</param>
    /// <returns> список складов </returns>
    public Task<IEnumerable<WarehouseResponseDto>> GetPaged(int Skip, int Take);

    /// <summary>
    /// Получить склад по Id
    /// </summary>
    /// <param name="id"> идентификатор </param>
    /// <returns> Id склада </returns>
    public Task<WarehouseResponseDto> GetById(Guid id);

    /// <summary>
    /// Создать склад
    /// </summary>
    /// <param name="dto">DTO создания склада</param>
    /// <returns> идентификатор нового склада </returns>
    public Task<Guid> Create(WarehouseCreateDto dto);

    /// <summary>
    /// Удалить склад
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <returns>true: удален, false: не найден </returns>
    public Task<bool> Delete(Guid id);

    /// <summary>
    /// Изменить склад
    /// </summary>
    /// <param name="dto">DTO обновления склада</param>
    /// <returns>true: обновлен, false: не найден </returns>
    public Task<bool> Update(WarehouseUpdateDto dto);
    public Task<WarehouseResponseDto> GetByParentId(Guid ParentId);
}
