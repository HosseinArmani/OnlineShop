using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site
{
    public class EditSliderViewModel : CreateSliderViewModel
    {
        public long SliderId { get; set; }
        [Display(Name = "تصویر اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string SliderImageName { get; set; }

    }
    public enum EditSliderResult
    {
        Success,
        NotFound
    }
}
