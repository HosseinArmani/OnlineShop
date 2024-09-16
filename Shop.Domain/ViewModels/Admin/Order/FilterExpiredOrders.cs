using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Order
{
   public class FilterExpiredOrders:BasePaging
    {
        public List<Models.Orders.Order> Orders { get; set; }


        #region methods
        public FilterExpiredOrders SetOrders(List<Models.Orders.Order> orders)
        {
            this.Orders = orders;
            return this;
        }

        public FilterExpiredOrders SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.CountForShowAfterAndBefor = paging.CountForShowAfterAndBefor;
            this.SkipEntitiy = paging.SkipEntitiy;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }
}
