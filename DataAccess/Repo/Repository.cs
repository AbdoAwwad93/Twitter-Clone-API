﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public IEnumerable<T> GetAll(Expression<Func<T, object>> selector, Expression<Func<object, bool>> match = null!)
        {
            IQueryable<T> query = (IQueryable<T>)dbSet.Select(selector);
            if (match==null)
            {
                return query;
            }
            return (IEnumerable<T>)query.Where(match).ToList();
        }
        public T GetById(string id)
        {
            return dbSet.Find(id)!;
        }
        public T GetById(int id)
        {
            return dbSet.Find(id)!;
        }
        public void Update(T entity)
        {
          dbSet.Update(entity);
        }
    }
}
