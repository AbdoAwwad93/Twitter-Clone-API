using TwitterClone_API.Models.AppModels;

namespace TwitterClone_API.DataAccess.Repo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IRepository<AppUser> APPUsers { get; private set; }
        public IRepository<Tweet> Tweets { get; private set; }
        public IRepository<Comment> Comments { get; private set; }
        public IRepository<LikedTweet> LikedTweets { get; private set; }
        public IRepository<LikedComment> LikedComments { get; private set; }
        public IFollowRepository Follows { get; private set; }  
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            APPUsers = new Repository<AppUser>(_context);
            Tweets = new Repository<Tweet>(_context);
            Comments = new Repository<Comment>(_context);
            LikedTweets = new Repository<LikedTweet>(_context);
            LikedComments = new Repository<LikedComment>(_context);
            Follows = new FollowRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}