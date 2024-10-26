
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Campus
    {
        public int CampusID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CampusName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Province { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string CampusChief { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactInfo { get; set; }
        public int LoginCode { get; set; }

    }
}
