using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Order
{
   public class EditTransportationViewModel
    {
        public long Id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }
        [Display(Name = "مبلغ کرایه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public int Amount { get; set; }
        [Display(Name = "حداقل خرید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        
        public int minBuy { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
    }
    public enum EditTransportationResult
    {
        NotFound,
        Success
    }
}
