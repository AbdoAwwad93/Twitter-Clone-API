using Microsoft.EntityFrameworkCore;
using TwitterClone_API.Models.AppModels;

namespace TwitterClone_API.DataAccess.Repo
{
    public class FollowRepository :Repository<Follow>,IFollowRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Follow> dbset;
        public FollowRepository(AppDbContext context)
         :base(context)
        {
            _context=context;
            dbset=_context.Set<Follow>();
        }
        //public Follow GetById(string FollwerId,string FollowdId)
        //{
        //    return dbset
        //        .Include(f => f.FollowerUser)
        //        .Include(f => f.FollowedUser)
        //        .FirstOrDefault(f => f.Id == id)!;
        //}
        public List<AppUser> GetFollowers(string userId)
        {
            return dbset
              .Where(f => f.FollowedId == userId)
              .Select(f => f.FollowerUser)
              .ToList();
        }
       
        public List<AppUser> GetFollowings(string userId)
        {
            return dbset
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowedUser)
                .ToList();
        }

        public List<AppUser> GetMutualFollowers(string userId, string searchUserId)
        {
            var mutualFollowers = dbset
                .Where(f => f.FollowerId == userId)
                .Where(f => dbset.Any(s => s.FollowerId == searchUserId && s.FollowedId == f.FollowedId))
                .Select(f => f.FollowedUser)
                .ToList();
            return mutualFollowers;
        }

        public bool IsFollowing(string followerId, string followedId)
        {
            return dbset
                .Any(f => f.FollowerId == followerId && f.FollowedId == followedId);
        }
        public bool HasFollowers(AppUser user)
        {
            return user.Followers.Any();
        }
        public bool HasFollowing(AppUser user)
        {
            return user.Followings.Any();
        }
        public bool IsFollowed(AppUser user,string follwedUserId)
        {
            return user.Followings.Any(f => f.FollowedId == follwedUserId);
        }
    }
}