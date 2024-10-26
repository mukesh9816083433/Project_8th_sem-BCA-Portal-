using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Project.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{5,}$", ErrorMessage = "Username must start with a letter and be at least 6 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Province is required")]
        [Range(1, 7, ErrorMessage = "Province must be a number between 1 and 7.")]
        public String Province { get; set; }

        [Required(ErrorMessage = "District is required")]
        [RegularExpression(@"^[a-zA-Z].*", ErrorMessage = "District must start with a letter.")]
        public string District { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}

//}
//using System.ComponentModel.DataAnnotations;
//using System.Text.RegularExpressions;

//namespace Project.ViewModels
//{
//    public class RegisterViewModel
//    {
//        [Required(ErrorMessage = "Username is required")]
//        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{5,}$", ErrorMessage = "Username must start with a letter and be at least 6 characters long.")]
//        public string Username { get; set; }

//        [Required(ErrorMessage = "Password is required")]
//        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
//            ErrorMessage = "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
//        public string Password { get; set; }

//        [Required(ErrorMessage = "Please confirm your password")]
//        [Compare("Password", ErrorMessage = "Passwords do not match")]
//        public string ConfirmPassword { get; set; }

//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email address")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Province is required")]
//        [Range(1, 7, ErrorMessage = "Province must be a number between 1 and 7.")]
//        public int Province { get; set; }

//        [Required(ErrorMessage = "District is required")]
//        [RegularExpression(@"^[a-zA-Z].*", ErrorMessage = "District must start with a letter.")]
//        public string District { get; set; }

//        [Required(ErrorMessage = "Address is required")]
//        public string Address { get; set; }
//    }
//}
