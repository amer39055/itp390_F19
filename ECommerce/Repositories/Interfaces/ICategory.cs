using Ecommerce.Models;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.Interfaces
{
    public interface ICategory
    {
        IList<Category> List();
        Category Find(int id);
        void Add(Category entity);
        void Update(int id, Category entity);
        void Delete(int id);
        List<Category> Search(string term);
    }
}
