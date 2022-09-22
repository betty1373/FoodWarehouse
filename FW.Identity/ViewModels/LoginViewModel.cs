using System.ComponentModel.DataAnnotations;

namespace FW.Identity.ViewModels
{
    public class LoginViewModel
    {
        [Required, MaxLength(256)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
