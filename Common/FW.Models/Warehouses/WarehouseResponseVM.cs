namespace FW.Models;
public class WarehouseResponseVM : IEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
}

