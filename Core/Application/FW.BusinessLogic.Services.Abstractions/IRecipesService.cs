using FW.BusinessLogic.Contracts.Recipes;

namespace FW.BusinessLogic.Services.Abstractions;

public interface IRecipesService
{
    /// <summary>
    /// Получить список изменений продуктов
    /// </summary>
    /// <returns> список изменений продуктов </returns>
    public Task<IEnumerable<RecipeResponseDto>> GetAll();
    /// <summary>
    /// Получить количество всех категорий
    /// </summary>
    /// <returns> количество всех категорий </returns>
    public Task<int> Count();
    /// <summary>
    /// Получить список рецептов
    /// </summary>
    /// <param name="pageNumber">номер страницы</param>
    /// <param name="pageSize">объем страницы</param>
    /// <returns> список рецептов </returns>
    public Task<IEnumerable<RecipeResponseDto>> GetPaged(int Skip, int Take);


    /// <summary>
    /// Получить рецепт по DishId
    /// </summary>
    /// <param name="id"> идентификатор </param>
    /// <returns> Id рецепта </returns>
    public Task<IEnumerable<RecipeResponseDto>> GetByParentId(Guid ParentId);

    /// <summary>
    /// Получить рецепт по Id
    /// </summary>
    /// <param name="id"> идентификатор </param>
    /// <returns> Id рецепта </returns>
    public Task<RecipeResponseDto> GetById(Guid id);

    /// <summary>
    /// Создать рецепт
    /// </summary>
    /// <param name="dto">DTO создания рецепта</param>
    /// <returns> идентификатор нового рецепта </returns>
    public Task<Guid> Create(RecipeCreateDto dto);

    /// <summary>
    /// Удалить рецепт
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <returns>true: удален, false: не найден </returns>
    public Task<bool> Delete(Guid id);

    /// <summary>
    /// Изменить рецепт
    /// </summary>
    /// <param name="dto">DTO обновления рецепта</param>
    /// <returns>true: обновлен, false: не найден </returns>
    public Task<bool> Update(RecipeUpdateDto dto);
}
