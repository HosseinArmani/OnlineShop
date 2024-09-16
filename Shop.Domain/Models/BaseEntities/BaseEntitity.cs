using System;
using System.ComponentModel.DataAnnotations;


namespace Shop.Domain.Models.BaseEntities
{
    public class BaseEntitity
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "حذف")]
        public bool IsDelete { get; set; }
        [Display(Name = "تاریخ")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
