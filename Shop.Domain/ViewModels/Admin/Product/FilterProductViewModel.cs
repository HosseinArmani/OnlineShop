using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Product
{
   public class FilterProductViewModel:BasePaging
    {
        public string ProductName { get; set; }
        public string FilterByGroup { get; set; }
        public string TitleInBrowser { get; set; }
        public List<Shop.Domain.Models.Production.Product> Products { get; set; }
        public List<ProductListItemViewModel> ProductListItemViewModel { get; set; }
        public List<long> SelectedGroups { get; set; } 
        public ProductState ProductState { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public ProductBox ProductBox { get; set; }

        #region method
        public FilterProductViewModel SetProducts(List<Shop.Domain.Models.Production.Product> products)
        {
            this.Products = products;
            return this;
        }
        public FilterProductViewModel SetProductsItem(List<ProductListItemViewModel> productItem)
        {
            this.ProductListItemViewModel = productItem;
            return this;
        }
        public FilterProductViewModel SetPaging(BasePaging basePaging)
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
    public enum ProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال ها")]
        IaActive,
        [Display(Name = "حذف شده ها")]
        Delete
    }
    public enum ProductOrder
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "جدید ترین ها")]
        ProductNews,
        [Display(Name = "گران ترین ها")]
        ProductExp,
        [Display(Name = "ارزان ترین ها")]
        ProductInExp
    }
    public enum ProductBox
    {
        Default,
        ItemBoxInSite
    }

}
