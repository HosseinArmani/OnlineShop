using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Domain.Models.Production;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IProductRepository
    {

        #region ProductGroup
        Task<List<ProductGroup>> GetAllProductGroup();
        Task<bool> ChechUrlNameGroup(string urlName);
        Task<bool> ChechUrlNameGroup(string urlName,long groupId); 
        Task AddProductGroup(ProductGroup group);
        void UpdateProductGroup(ProductGroup group);
        Task<ProductGroup>GetProductGroupById(long groupId);
        #endregion
        #region Product
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filter);
        Task<List<Product>> GetAllProduct();
        Task<List<SelectListItem>> GetGroupManageProduct();
        Task<List<SelectListItem>> GetSubGroupManageProduct(long groupId);
        Task AddProduct(Product product);
        Task<Product> GetProductById(long productId);
        void UpdateProduct(Product product);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);
        Task<List<ProductListItemViewModel>> ShowPopularProduct();
        Task<List<ProductListItemViewModel>> LatestProducts();
        Task<ProductDetailViewModel> ProductDetails(long productId);
        Task<List<ProductListItemViewModel>> RelatedProducts(string urlName,long productId);
        #endregion

        #region ProductGallery

        Task AddproductGalleries(List<ProductGallery> productGalleries);
        Task<bool> CheckProduct(long productId);
        Task<List<ProductGallery>> GetAllProductGalleries(long productId);
        Task<ProductGallery> GetProductGalleryById(long id);
        Task DeleteProductGallery(long id);
        #endregion

        #region ProductFeature
        Task<List<ProductFeature>> GetAllProductFeatures(long productId);
        Task AddProductFeature(ProductFeature product);
        Task DeleteFeature(long id);
        #endregion
        #region ProductComment
        Task<FilterCommentViewModel> FilterCommentProduct(FilterCommentViewModel filter);
        Task<ProductComment> GetCommentById(long commentId);
        void UpdateComment(ProductComment productComment);
        Task AddProductComment(ProductComment productComment);
        Task<List<ProductComment>> GetProductComment(long productId);
        Task DeleteComment(long id);
        #endregion
        #region ProductTag
        Task<List<ProductTag>> GetAllProductTag(long productId);
        Task AddProductTag(ProductTag tag);
        Task DeleteTag(long id); 
        #endregion
        Task SaveChange();
      
    }
}
