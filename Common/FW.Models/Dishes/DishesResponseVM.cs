
namespace FW.Models;
public class DishResponseVM : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

