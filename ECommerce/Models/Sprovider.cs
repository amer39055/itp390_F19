using System;
using System.Collections.Generic;

namespace ECommerce.Models
{
    public partial class Sprovider
    {
        public Sprovider()
        {
            Service = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public int? Rating { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<Service> Service { get; set; }
    }
}
