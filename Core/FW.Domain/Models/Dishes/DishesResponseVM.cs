namespace FW.Domain.Models;
public class DishResponseVM : IEntity
    {
        public Guid Id { get; set; }
     //   public DateTime ModifiedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

