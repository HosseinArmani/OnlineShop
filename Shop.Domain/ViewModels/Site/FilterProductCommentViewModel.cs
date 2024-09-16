using Shop.Domain.Models.Production;
using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site
{
   public class FilterProductCommentViewModel: BasePaging
    {
        public long ProductId { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public FilterProductCommentViewModel SetComment(List<ProductComment> ProductComment)
        {
            this.ProductComments = ProductComment;
            return this;
        }
        #region method

        public FilterProductCommentViewModel SetPaging(BasePaging basePaging)
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

        #endregion
    }
}
