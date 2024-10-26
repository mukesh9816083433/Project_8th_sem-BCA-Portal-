using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

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

        [MaxLength(200)]
        public string Collage { get; set; }
        public bool IsActive { get; set; }
    }
}
