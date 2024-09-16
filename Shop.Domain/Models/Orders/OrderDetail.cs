using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Orders
{
  public  class OrderDetail:BaseEntitity
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long OrderId  { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Count { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }


        #region Nav
        public Order Order { get; set; }
        public Product Product { get; set; }
        #endregion


    }
}
