using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Interfaces
{
    public interface IOrder
    {
        void Add(Order entity);
        void Delete(int id);
        Order Find(int id);
        IList<Order> List();
        void Update(int id, Order entity);
    }
}
