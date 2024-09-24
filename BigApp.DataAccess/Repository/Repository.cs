using BigApp.DataAccess.Data;
using BigApp.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BigApp.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> dbSet;
        private readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db) { 
            _db = db;
            this.dbSet = _db.Set<T>();
            //_db.Categories == dbSet
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            var query = dbSet;
            query.Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            var query = dbSet;
            query.RemoveRange(entity);
        }
    }
}
