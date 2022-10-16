using FW.BusinessLogic.Contracts.Category;
using FW.ResponseStatus;
namespace FW.BusinessLogic.Services.Abstractions
{
    public interface ICategoriesService
    {
        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <returns> список всех категорий </returns>
        public Task<IEnumerable<CategoryResponseDto>> GetAll();
        /// <summary>
        /// Получить список категорий
        /// </summary>
        /// <param name="pageNumber">номер страницы</param>
        /// <param name="pageSize">объем страницы</param>
        /// <returns> список категорий </returns>
        public Task<IEnumerable<CategoryResponseDto>> GetPaged(int Skip, int Take);

        /// <summary>
        /// Получить категорию по Id
        /// </summary>
        /// <param name="id"> идентификатор </param>
        /// <returns> Id категории </returns>
        public Task<CategoryResponseDto> GetById(Guid id);

        /// <summary>
        /// Получить количество всех категорий
        /// </summary>
        /// <returns> количество всех категорий </returns>
        public Task<int> Count();

        /// <summary>
        /// Создать категорию
        /// </summary>
        /// <param name="dto">DTO категории</param>
        /// <returns> идентификатор новой категории </returns>
        public Task<Guid> Create(CategoryCreateDto dto);

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>true: удалена, false: не найдена </returns>
        public Task<bool> Delete(Guid id);

        /// <summary>
        /// Изменить категорию
        /// </summary>
        /// <param name="dto">DTO обновления категории</param>
        /// <returns>true: обновлена, false: не найдена </returns>
        public Task<bool> Update(CategoryUpdateDto dto);
    }
}
