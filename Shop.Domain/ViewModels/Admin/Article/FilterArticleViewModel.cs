using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Article
{
   public class FilterArticleViewModel: BasePaging
    {
        public string ArticlTitle{ get; set; }
        public string TitleInBrowser { get; set; }
        public List<Shop.Domain.Models.Articles.Article> Articles { get; set; }

        #region method
        public FilterArticleViewModel SetArticle(List<Shop.Domain.Models.Articles.Article> articles)
        {
            this.Articles = articles;
            return this;
        }
      
        public FilterArticleViewModel SetPaging(BasePaging basePaging)
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
