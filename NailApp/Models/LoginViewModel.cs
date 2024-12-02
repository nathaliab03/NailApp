using System.ComponentModel.DataAnnotations;

namespace NailApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Lembre-se de mim")]
        public bool RememberMe { get; set;}

        public string Role { get; } = "Admin";
    }
}
