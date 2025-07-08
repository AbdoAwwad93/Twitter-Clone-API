using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone_API.Models.AppModels
{
    public class LikedTweet
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("Tweet")]
        public int TweetId { get; set; }
        public virtual Tweet Tweet { get; set; }
        public DateTime LikedAt { get; set; }

    }
}
