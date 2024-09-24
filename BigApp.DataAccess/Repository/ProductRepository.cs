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
    public class productRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public productRepository(ApplicationDbContext db) : base(db)   // this code base(db) add in base class Repository  
        {   
               _db = db;
        }

        public void Update(Product obj)
        { 
           _db.Products.Update(obj);
        }

       
    }
}
