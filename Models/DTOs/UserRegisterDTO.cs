using System.ComponentModel.DataAnnotations;

namespace TwitterClone_API.Models.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName Is required")]
        
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Date of Birth is Required")]
        public DateOnly DateOfBirth { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}
