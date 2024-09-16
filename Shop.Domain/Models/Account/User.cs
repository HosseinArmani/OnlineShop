using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Orders;
using Shop.Domain.Models.Production;
using Shop.Domain.Models.Site;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shop.Domain.Models.Account
{
  public  class User: BaseEntitity
    {
        [Display(Name = "نام ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = " موبایل ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }
        [Display(Name = "مسدود ")]
        public bool Isblocked { get; set; }
        [Display(Name = " وضعیت اکانت")]
        public bool IsActive { get; set; }
        [Display(Name = "کدفعالسازی")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ActiveCode { get; set; }


        #region Navigation
        public ICollection<UserWallet> UserWallets { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public ICollection <Order> Orders { get; set; }
        public ICollection<Address> addresses{ get; set; }
        public ICollection<UserDiscount> UserDiscounts { get; set; }
        public ICollection<UserFavorites> UserFavorites { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        #endregion

    }
}
