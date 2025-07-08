using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone_API.Models.AppModels
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string CommnetText { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("Tweet")]
        public int TweetId { get; set; }
        public virtual Tweet Tweet { get; set; }
    }
}
