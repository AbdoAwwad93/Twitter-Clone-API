using Microsoft.EntityFrameworkCore;

namespace TwitterClone_API.DataAccess.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(AppDbContext _context)
        {
            context=_context;
            dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public List<T> GetAll()
        {
           return dbSet.ToList();
        }

        public T GetById(string id)
        {
            return dbSet.Find(id)!;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
          dbSet.Update(entity);
        }
    }
}
