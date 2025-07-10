using System.ComponentModel.DataAnnotations;

namespace TwitterClone_API.Models.DTOs
{
    public class EditProfileDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters")]
        public string FirstName { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [StringLength(200)]
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public List<TweetDTO> Tweets { get; set; }
    }
}
