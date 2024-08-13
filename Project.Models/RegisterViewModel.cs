using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Province { get; set; }

        [Required]
        [MaxLength(50)]
        public string District { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
    }
}
