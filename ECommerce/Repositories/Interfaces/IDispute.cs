using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Interfaces
{
    public interface IDispute
    {
        IList<Dispute> List();
        Dispute Find(int id);
        void Add(Dispute entity);
        void Update(int id, Dispute entity);
        void Delete(int id);
    }
}
