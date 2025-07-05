using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterClone_API.Models;

namespace TwitterClone_API.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikedTweet> LikedTweets { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().HasMany(user => user.Tweets)
                .WithOne(tweet => tweet.User);

            modelBuilder.Entity<AppUser>().HasMany(user => user.Comments)
                .WithOne(comment => comment.User);
            modelBuilder.Entity<AppUser>().HasMany(user => user.UserLikes).WithOne(likedTweet => likedTweet.User);

            modelBuilder.Entity<Follow>().HasOne(Userf => Userf.FollowerUser)
                .WithMany(user => user.Followings);

            modelBuilder.Entity<Follow>().HasOne(Userf => Userf.FollowedUser)
                .WithMany(user => user.Followers);

            modelBuilder.Entity<Tweet>().HasMany(tweet => tweet.Comments)
                .WithOne(comment => comment.Tweet);

            modelBuilder.Entity<Tweet>().HasMany(tweet => tweet.Likes)
                .WithOne(likedTweet => likedTweet.Tweet);



        }

    }
}
