using System.ComponentModel.DataAnnotations.Schema;

namespace FW.Domain;

public class ChangesProducts : Base
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("ProductId")]
    public Products Products { get; set; }
}
