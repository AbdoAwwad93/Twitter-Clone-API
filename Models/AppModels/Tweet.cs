using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TwitterClone_API.Models.AppModels
{
    public class Tweet
    {
        public int Id { get; set; }
        public string TweetText { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<LikedTweet>? Likes { get; set; }
    }
}
