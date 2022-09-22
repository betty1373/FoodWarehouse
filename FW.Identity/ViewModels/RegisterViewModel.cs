using System.ComponentModel.DataAnnotations;

namespace FW.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(256)]
        public string Username { get; set; }
        
        [Required,MaxLength(256)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
