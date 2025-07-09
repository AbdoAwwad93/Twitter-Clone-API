using System.ComponentModel.DataAnnotations;

namespace TwitterClone_API.Models.DTOs
{
    public class AddCommentDTO
    {
        [Required]
        public string CommentText { get; set; }
    }
}
