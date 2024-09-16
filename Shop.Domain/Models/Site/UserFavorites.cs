using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Site
{
  public  class UserFavorites:BaseEntitity
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }

        #region Nav
        public User User { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}
