using Ecommerce.Models;
using Ecommerce.Repositories.Interfaces;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories
{
    public class CategoryRepository : ICategory
    {
        myDbContext db;
        public CategoryRepository(myDbContext _db)
        {
            db = _db;
        }
        public void Add(Category entity)
        {
            db.Category.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = Find(id);

            db.Category.Remove(category);
            db.SaveChanges();
        }

        public Category Find(int id)
        {
            var category = db.Category.SingleOrDefault(a => a.Id == id);

            return category;
        }

        public IList<Category> List()
        {
            return db.Category.ToList();
        }

        public List<Category> Search(string term)
        {
            return db.Category.Where(a => a.Name.Contains(term)).ToList();
        }

        public void Update(int id, Category entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
