using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }
        [Display(Name = "الاسم الاول")]
        public string FirstName { get; set; }
        [Display(Name = "الكنية")]
        public string LastName { get; set; }
        [Display(Name = "تاريخ الميلاد")]
        public string BirthDate { get; set; }
        [Display(Name = "الرقم الوطني")]
        public string NationalId { get; set; }
        [Display(Name = "المدينة")]
        public string City { get; set; }
        [Display(Name = "العنوان")]
        public string Address { get; set; }
        [Display(Name = "الموقع")]
        public string HomeLocation { get; set; }
        [Display(Name = "النوع")]
        public string Gender { get; set; }
        [Display(Name = "نوع المتخدم")]
        public string UserType { get; set; }
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
        [Display(Name = "الحالة")]
        public string Status { get; set; } = "Active";
    }
}
