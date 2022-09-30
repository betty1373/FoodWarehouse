using FW.BusinessLogic.Contracts.Ingredients;
using FW.BusinessLogic.Contracts.Products;

namespace FW.BusinessLogic.Services.Abstractions
{
    public interface IProductsService
    {
        /// <summary>
        /// Получить список изменений продуктов
        /// </summary>
        /// <returns> список изменений продуктов </returns>
        public Task<IEnumerable<ProductResponseDto>> GetAll();
        /// <summary>
        /// Получить количество всех категорий
        /// </summary>
        /// <returns> количество всех категорий </returns>
        public Task<int> Count();
        /// <summary>
        /// Получить список продуктов
        /// </summary>
        /// <param name="pageNumber">номер страницы</param>
        /// <param name="pageSize">объем страницы</param>
        /// <returns> список продуктов </returns>
        public Task<IEnumerable<ProductResponseDto>> GetPaged(int Skip, int Take);


        /// <summary>
        /// Получить продукты по складу 
        /// </summary>
        /// <param name="id"> идентификатор </param>
        /// <returns> Id рецепта </returns>
        public Task<IEnumerable<ProductResponseDto>> GetByParentId(Guid ParentId);

        /// <summary>
        /// Получить продукт по Id
        /// </summary>
        /// <param name="id"> идентификатор </param>
        /// <returns> Id продукта </returns>
        public Task<ProductResponseDto> GetById(Guid id);

        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="dto">DTO создания продукта</param>
        /// <returns> идентификатор нового продукта </returns>
        public Task<Guid> Create(ProductCreateDto dto);

        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>true: удален, false: не найден </returns>
        public Task<bool> Delete(Guid id);

        /// <summary>
        /// Изменить продукт
        /// </summary>
        /// <param name="dto">DTO обновления продукта</param>
        /// <returns>true: обновлен, false: не найден </returns>
        public Task<bool> Update(ProductUpdateDto dto);
    }
}
