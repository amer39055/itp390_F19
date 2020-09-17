using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories
{
    public class DisputeRepository : IDispute
    {
        myDbContext db;

        public DisputeRepository(myDbContext _db)
        {
            db = _db;
        }

        public void Add(Dispute entity)
        {
            db.Dispute.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var dispute = Find(id);

            db.Dispute.Remove(dispute);
            db.SaveChanges();
        }

        public Dispute Find(int id)
        {
            var dispute = db.Dispute.Include(d => d.Arbiter).Include(d => d.Order).ThenInclude(d => d.Service).ThenInclude(d => d.Sprovider).ThenInclude(x => x.User).Include(x => x.Order).ThenInclude(x => x.Customer).SingleOrDefault(d => d.Id == id);

            return dispute;
        }

        public IList<Dispute> List()
        {
            return db.Dispute.Include(d => d.Order).ThenInclude(d=>d.Service).ThenInclude(d=>d.Sprovider).ThenInclude(d=>d.User).Include(d=>d.Order).ThenInclude(d=>d.Customer).Include(d => d.Arbiter).ToList();
        }

        public void Update(int id, Dispute entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
