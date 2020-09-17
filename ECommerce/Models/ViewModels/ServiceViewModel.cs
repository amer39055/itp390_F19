using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public int SproviderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public string ExpectedTime { get; set; }
        public IFormFile Image { get; set; }

        public Sprovider Sprovider { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
