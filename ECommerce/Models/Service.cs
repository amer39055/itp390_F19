using System;
using System.Collections.Generic;

namespace ECommerce.Models
{
    public partial class Service
    {
        public Service()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int SproviderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public string ExpectedTime { get; set; }
        public string Image { get; set; }

        public Sprovider Sprovider { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
