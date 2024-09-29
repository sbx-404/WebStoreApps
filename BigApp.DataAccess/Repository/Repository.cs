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

            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);            //By default, when you fetch Products, the related Category entity is not loaded automatically. To load we use .include()
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var product in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))         // split char into comma seprerated
                {
                    query = query.Include(product);              // add properties
                }
            }
                return query.FirstOrDefault();
        }

            public IEnumerable<T> GetAll(string? includeProperties = null)
            {
                IQueryable<T> query = dbSet;
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var product in includeProperties
                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))         // split char into comma seprerated
                    {
                        query = query.Include(product);              // add properties
                    }
                }
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
