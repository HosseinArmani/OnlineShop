﻿using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Site
{
    public class Slider : BaseEntitity
    {
        [Display(Name = "تصویر اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string SliderImageName { get; set; }
        [Display(Name = "عنوان اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string SliderTitle { get; set; }
        [Display(Name = "ادرس اسلایدر")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Url { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }



    }
}
