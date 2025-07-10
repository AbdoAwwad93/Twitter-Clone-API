using TwitterClone_API.Models.AppModels;

namespace TwitterClone_API.DataAccess.Repo
{
    public interface IFollowRepository : IRepository<Follow>
    {
        List<AppUser> GetMutualFollowers(string userId, string searchUserId);
        List<AppUser> GetFollowers(string userId);
        List<AppUser> GetFollowings(string userId);
        bool IsFollowing(string followerId, string followedId);
        bool HasFollowing(AppUser user);
        bool HasFollowers(AppUser user);
        public bool IsFollowed(AppUser user, string follwedUserId);
    }
}