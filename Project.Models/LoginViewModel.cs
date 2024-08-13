using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
