using FW.BusinessLogic.Contracts;

namespace FW.BusinessLogic.Services.Abstractions
{
    public interface ICookingDishService
    {
        /// <summary>
        /// Приготовить блюдо
        /// </summary>
        /// <param name="dishId">идентификатор блюда</param>
        /// <param name="numPortions">количество порций</param>
        /// <returns>true: приготовлено, false: нет </returns>
        public Task<ResponseStatusResult> Cook(Guid dishId, Guid userId, int numPortions);
    }
}
