using Shop.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Account
{
  public class UserDiscount
    {
        [Key]
        public long UD_Id { get; set; }
        public long UserId { get; set; }
        public long DisCountId { get; set; }

        #region Nav
        public Discount Discount { get; set; }
        public User User { get; set; }

        #endregion
    }
}
