using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Account
{
    public class UserWallet : BaseEntitity
    {
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long UserId { get; set; }
        [Display(Name = " نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public WalletType WalletType { get; set; }
        [Display(Name = "مبلغ ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }
        [Display(Name = "شرح ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; }
        [Display(Name = "پرداخت شده ")]
        public bool IsPay { get; set; }

        #region Navigation
        public User User { get; set; }
        #endregion

    }
    public enum WalletType
    {
        [Display(Name = "واریز ")]
        Variz = 1,
        [Display(Name = " برداشت")]
        Bardasht = 2
    }
}
