using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Order
{
  public class EditDiscountCodeViewModel
    {
        public long DiscountId { get; set; } 
        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DisCountCode { get; set; }
        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Percent { get; set; }
        public int? UsabaleCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StDate { get; set; }
        public string EdDate { get; set; }
    }
    public enum EditDiscountCodeResult
    {
        NotFound,
        Success
    }
}
