using System.ComponentModel.DataAnnotations.Schema;

namespace FW.Domain
{
    public class Warehouses : Base
    {
        public string Name { get; set; }
        public string Address { get; set; }
      //  public Guid UserId { get; set; }

        //[ForeignKey("UserId")]
        //public Users User { get; set; }
    }
}
