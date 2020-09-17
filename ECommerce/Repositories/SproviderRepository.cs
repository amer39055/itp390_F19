using Ecommerce.Models;
using Ecommerce.Repositories.Interfaces;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories
{
    public class SproviderRepository : ISprovider
    {
        myDbContext db;
        public SproviderRepository(myDbContext _db)
        {
            db = _db;
        }
        public void Add(Sprovider entity)
        {
            db.Sprovider.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var ServicesProvider = Find(id);

            db.Sprovider.Remove(ServicesProvider);
            db.SaveChanges();
        }

        public Sprovider Find(int id)
        {
            var Sprovider = db.Sprovider.Include(s => s.Category).Include(s => s.User).SingleOrDefault(a => a.Id == id);

            return Sprovider;
        }

        public IList<Sprovider> List()
        {
            return db.Sprovider.Include(s => s.Category).Include(s => s.User).ToList();
        }

        public void Update(int id, Sprovider entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
