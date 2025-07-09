using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace TwitterClone_API.Models.AppModels
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<Tweet>? Tweets { get; set; }
        public virtual List<LikedTweet>? UserLikes { get; set; }
        public virtual List<Follow> Followers { get; set; }
        public virtual List<Follow> Followings { get; set; }
    }
}
