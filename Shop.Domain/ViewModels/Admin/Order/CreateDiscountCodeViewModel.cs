using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Order
{
   public class CreateDiscountCodeViewModel
    {
        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DisCountCode { get; set; }
        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Percent { get; set; }
        public int? UsabaleCount { get; set; }
        public string StDate { get; set; }
        public string EdDate { get; set; }
    }
    public enum CreateDiscountCodeResult
    {
        Sucesss
    }
}
