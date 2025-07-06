namespace TwitterClone_API.DataAccess.Repo
{
    public interface IRepository<T> where T : class
    {
        public void Add(T entity);
        public void delete(T entity);
        public void Update(T entity);
        public List<T> GetAll();
        public T GetById(string id);
        public void Save();
    }
}
