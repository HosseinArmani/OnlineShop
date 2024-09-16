using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Product
{
   public class FilterCommentViewModel :BasePaging
    {
        public List<Shop.Domain.Models.Production.ProductComment> ProductComments { get; set; }

        public FilterCommentViewModel SetProductComments(List<Shop.Domain.Models.Production.ProductComment> productComments)
        {
            this.ProductComments = productComments;

            return this;
        }
        public FilterCommentViewModel SetPaging(BasePaging basePaging)
        {
            this.PageId = basePaging.PageId;
            this.PageCount = basePaging.PageCount;
            this.AllEntityCount = basePaging.AllEntityCount;
            this.CountForShowAfterAndBefor = basePaging.CountForShowAfterAndBefor;
            this.EndPage = basePaging.EndPage;
            this.SkipEntitiy = basePaging.SkipEntitiy;
            this.StartPage = basePaging.StartPage;
            this.TakeEntity = basePaging.TakeEntity;

            return this;

        }
    }

}
