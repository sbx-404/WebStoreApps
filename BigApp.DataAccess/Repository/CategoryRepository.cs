using BigApp.DataAccess.Data;
using BigApp.DataAccess.Repository.IRepository;
using BigApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BigApp.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)   // this code base(db) add in base class Repository  
        {   
               _db = db;
        }

        public void Update(Category obj)
        { 
           _db.Categories.Update(obj);
        }
       

    }
}
