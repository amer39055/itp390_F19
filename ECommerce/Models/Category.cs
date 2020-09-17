using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public partial class Category
    {
        public Category()
        {
            Sprovider = new HashSet<Sprovider>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name="فئة الخدمة")]
        public string Name { get; set; }
        [Display(Name="وصف الفئة")]
        public string Description { get; set; }
        public string Image { get; set; }

        public ICollection<Sprovider> Sprovider { get; set; }
    }
}
