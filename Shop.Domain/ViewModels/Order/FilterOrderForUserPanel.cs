using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Order
{
   public class FilterOrderForUserPanel:BasePaging
    {
        public long UserId { get; set; }
        public List<Models.Orders.Order> Orders { get; set; }


        #region methods
        public FilterOrderForUserPanel SetOrders(List<Models.Orders.Order> orders)
        {
            this.Orders = orders;
            return this;
        }

        public FilterOrderForUserPanel SetPaging(BasePaging paging)
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
