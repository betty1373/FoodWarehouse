namespace FW.BusinessLogic.Contracts.Category
{
    public class CategoryUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
