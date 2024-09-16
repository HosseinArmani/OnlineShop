using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Domain.Models.Production;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shop.Application.Interfaces
{
    public interface IProductService
    {
        #region ProductGroup
        Task<List<ProductGroup>> GetAllProductGroup();
        Task<CreateProductGroupRresult> CreateProductGroup(CreateProductGroupViewModel createGroup, IFormFile imageGroup, long? id);
        Task<EditProducGroupResult> EditProducGroup(EditProducGroupViewModel editGroup, IFormFile imageGroup);
        Task<EditProducGroupViewModel> GetEditProducGroup(long GroupId);

        #endregion


        #region Product

        Task<List<SelectListItem>> GetGroupManageProduct();
        Task<List<SelectListItem>> GetSubGroupManageProduct(long groupId);
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filter);
        Task<List<Product>> GetAllProduct();
        Task<CreateProductResult> CreateProduct(CreateProductViewModel create, IFormFile imgProduct);
        Task<EditProductViewModel> GetEditProduct(long productId);
        Task<EditProductReult> EditProduct(EditProductViewModel editProduct, IFormFile imgProduct);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);
        Task<List<ProductListItemViewModel>> ShowPopularProduct();
        Task<List<ProductListItemViewModel>> LatestProducts();
        Task<ProductDetailViewModel> ProductDetails(long productId);
        Task<List<ProductListItemViewModel>> RelatedProducts(string urlName, long productId);

        #endregion

        #region  ProductGallery

        Task<bool> AddProductGallery(long productId, List<IFormFile> imgProducts);
        Task<List<ProductGallery>> GetAllProductGalleries(long productId);
        Task DeleteImageGallery(long galleryId);


        #endregion

        #region ProductFeature
        Task<List<ProductFeature>> GetAllProductFeatures(long productId);
        Task<ProductFeatureResult> CreateProductFeature(CreateProductFeatureViewModel productFeature);
        Task DeleteFeature(long id);

        #endregion

        #region ProductComment
        Task<FilterCommentViewModel> FilterCommentProduct(FilterCommentViewModel filter);
        Task ReadAdmin(long commentId);
        Task<CreateProductCommentResult> CreateProductComment(CreateProductCommentViewModel createComment,long userId);
        Task<List<ProductComment>> GetProductComment(long productId);
        Task DeleteComment(long id);

        #endregion

        #region ProductTag
        Task<CreateProductTagResult> CreateProductTag(CreateProductTagViewModel createTag);
        Task<List<ProductTag>> GetAllProductTag(long productId);
        Task DeleteTag(long id);
        #endregion
    }
}
