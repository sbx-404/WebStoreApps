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
            //_db.Products.Update(obj); 

            //This is explicitly tell what is saving in database.
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Author = obj.Author;
                objFromDb.Price100 = obj.Price100;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Description = obj.Description;
                objFromDb.Title = obj.Title;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Price = obj.Price;
                if (obj.ImageUrl != null)      // update ka mtlb ha ki pahla sa data hoga agar bhara ha tho. Tho usi data ko access karna
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }


        }
    }
}