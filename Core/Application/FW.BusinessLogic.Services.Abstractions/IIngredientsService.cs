using FW.BusinessLogic.Contracts.Dishes;
using FW.BusinessLogic.Contracts.Ingredients;

namespace FW.BusinessLogic.Services.Abstractions
{
    public interface IIngredientsService
    {
        /// <summary>
        /// Получить список изменений продуктов
        /// </summary>
        /// <returns> список изменений продуктов </returns>
        public Task<IEnumerable<IngredientResponseDto>> GetAll();

        /// <summary>
        /// Получить количество всех категорий
        /// </summary>
        /// <returns> количество всех категорий </returns>
        public Task<int> Count();
        /// <summary>
        /// Получить список ингредиентов
        /// </summary>
        /// <param name="pageNumber">номер страницы</param>
        /// <param name="pageSize">объем страницы</param>
        /// <returns> список ингредиентов </returns>
        public Task<IEnumerable<IngredientResponseDto>> GetPaged(int Skip, int Take);

        /// <summary>
        /// Получить ингредиент по Id
        /// </summary>
        /// <param name="id"> идентификатор </param>
        /// <returns> Id ингредиента </returns>
        public Task<IngredientResponseDto> GetById(Guid id);

        /// <summary>
        /// Создать ингредиент
        /// </summary>
        /// <param name="dto">DTO создания ингредиента</param>
        /// <returns> идентификатор нового ингредиента </returns>
        public Task<Guid> Create(IngredientCreateDto dto);

        /// <summary>
        /// Удалить ингредиент
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>true: удален, false: не найден </returns>
        public Task<bool> Delete(Guid id);

        /// <summary>
        /// Изменить ингредиент
        /// </summary>
        /// <param name="dto">DTO обновления ингредиента</param>
        /// <returns>true: обновлен, false: не найден </returns>
        public Task<bool> Update(IngredientUpdateDto dto);
    }
}
