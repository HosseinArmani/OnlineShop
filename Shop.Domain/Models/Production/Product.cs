using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Orders;
using Shop.Domain.Models.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Production
{
    public class Product : BaseEntitity
    {
        [Display(Name = "گروه اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long GroupId { get; set; }
        [Display(Name = "گروه فرعی")]
        public long? SubGroupId { get; set; }
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }
        [Display(Name = " عنوان مرورگر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string TitleInBrowser { get; set; }
        [Display(Name = "توضیحات کوتاه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }
        [Display(Name = "تعداد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public int Count { get; set; }
        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
        [Display(Name = "تصویر محصول")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public int? DicPercent { get; set; }



        #region Navigation
        public ICollection<ProductGallery> productGalleries { get; set; }
        [ForeignKey("GroupId")]
        public ProductGroup ProductGroup { get; set; }

        [ForeignKey("SubGroupId")]
        public ProductGroup SubGroup { get; set; }
        public  ICollection<ProductFeature> productFeatures { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<UserFavorites> UserFavorites { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; } 
        #endregion
    }
}
