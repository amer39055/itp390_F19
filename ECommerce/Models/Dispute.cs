using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public partial class Dispute
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [Display(Name="تاريخ البدء")]
        public DateTime OpenedDate { get; set; }
        public string ArbiterId { get; set; }
        [Display(Name = "المشتكي")]
        public string Complaint { get; set; }
        [Display(Name = "النتيجة")]
        public string Result { get; set; }
        [Display(Name = "الحالة")]
        public string Status { get; set; }
        [Display(Name = "الحكم")]
        public AspNetUsers Arbiter { get; set; }
  
        public Order Order { get; set; }
    }
}
