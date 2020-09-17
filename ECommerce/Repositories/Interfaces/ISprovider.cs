using Ecommerce.Models;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.Interfaces
{
    public interface ISprovider
    {
        void Add(Sprovider entity);
        void Delete(int id);
        Sprovider Find(int id);
        IList<Sprovider> List();
        void Update(int id, Sprovider entity);
    }
}
