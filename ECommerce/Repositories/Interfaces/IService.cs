using Ecommerce.Models;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.Interfaces
{
    public interface IService
    {
        void Add(Service entity);
        void Delete(int id);
        Service Find(int id);
        IList<Service> List();
        List<Service> Search(string term);
        void Update(int id, Service entity);
    }
}
