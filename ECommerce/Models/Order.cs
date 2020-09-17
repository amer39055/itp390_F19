using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public partial class Order
    {
        public Order()
        {
            Dispute = new HashSet<Dispute>();
        }

        public int Id { get; set; }

        public string CustomerId { get; set; }
        public int ServiceId { get; set; }
        [Display(Name ="حالة الطلب")]
        public string OrderStatus { get; set; }
        [Display(Name = "تاريخ الطلبية")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "التقييم")]
        public int? Rating { get; set; }
        [Display(Name = "تعليقات على الطلبية")]
        public string OrderNotes { get; set; }
        [Display(Name = "تعليقات على التقييم")]
        public string RatingNotes { get; set; }

        [Display(Name = "الزبون")]
        public AspNetUsers Customer { get; set; }
        public Service Service { get; set; }
        public ICollection<Dispute> Dispute { get; set; }
    }
}
