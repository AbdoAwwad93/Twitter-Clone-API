using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone_API.Models
{
    public class Follow
    {
        public int Id { get; set; }
        [ForeignKey("FollowerUser")]
        public string FollowerId { get; set; }
        public virtual AppUser FollowerUser { get; set; }
        [ForeignKey("FollowedUser")]
        public string FollowedId { get; set; }
        public virtual AppUser FollowedUser { get; set; }
        public DateTime FollowedAt { get; set; }

    }
}
