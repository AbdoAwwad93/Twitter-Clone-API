using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone_API.Models.AppModels
{
    public class LikedComment
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public DateTime LikedAt { get; set; }
    }
}
