using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public partial class Sprovider
    {
        public Sprovider()
        {
            Service = new HashSet<Service>();
        }

        public int Id { get; set; }
        [Display(Name = "الرقم الوطني")]
        public string UserId { get; set; }
        [Display(Name = "مقدم الخدمة")]
        public string CompanyName { get; set; }
        [Display(Name = "التقييم")]
        public int? Rating { get; set; }
        [Display(Name = "صورة")]
        public string Image { get; set; }
        [Display(Name = "رقم الفئة")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<Service> Service { get; set; }
    }
}
