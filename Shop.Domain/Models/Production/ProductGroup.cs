using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Production
{
    public class ProductGroup : BaseEntitity
    {
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string GroupTitle { get; set; }
        public long? ParentId { get; set; }

        [Display(Name = "عنوان Url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UrlName { get; set; }
        [Display(Name = "تصویر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; } 


        #region Navigation
        [ForeignKey("ParentId")]
        public ICollection<ProductGroup> ProductGroups { get; set; }
        [InverseProperty("ProductGroup")]
        public ICollection<Product> Products { get; set; }
        [InverseProperty("SubGroup")]
        public ICollection<Product> SubGroups { get; set; }
        #endregion

    }
}
