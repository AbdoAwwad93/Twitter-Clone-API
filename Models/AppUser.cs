using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace TwitterClone_API.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Tweet>? Tweets { get; set; }
        public List<LikedTweet>? UserLikes { get; set; }
        public List<Follow> Followers { get; set; }
        public List<Follow> Followings { get; set; }
    }
}
