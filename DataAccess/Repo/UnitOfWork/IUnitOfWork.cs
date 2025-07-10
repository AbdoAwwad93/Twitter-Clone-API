using TwitterClone_API.Models.AppModels;

namespace TwitterClone_API.DataAccess.Repo.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        public IRepository<AppUser> APPUsers { get;}
        public IRepository<Tweet> Tweets { get; }
        public IRepository<Comment> Comments { get; }
        public IRepository<LikedTweet> LikedTweets { get; }
        public IRepository<LikedComment> LikedComments { get; }
        public IFollowRepository Follows { get; }
        public void Save();
    }
}
