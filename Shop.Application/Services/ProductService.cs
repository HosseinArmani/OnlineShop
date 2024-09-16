using Ganss.Xss;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Production;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Shop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
      
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
          
        }

        #region ProductGroup
        public async Task<List<ProductGroup>> GetAllProductGroup()
        {
            return await _productRepository.GetAllProductGroup();
        }

        public async Task<CreateProductGroupRresult> CreateProductGroup(CreateProductGroupViewModel createGroup, IFormFile imageGroup, long? id)
        {
            if (await _productRepository.ChechUrlNameGroup(createGroup.UrlName)) return CreateProductGroupRresult.IsExitUrlName;
            var productGroup = new ProductGroup
            {
                UrlName = createGroup.UrlName,
                GroupTitle = createGroup.Title,
                ParentId = id
            };
            #region AddImage
            productGroup.ImageName = "shopping-cart.png";
            if (imageGroup != null && imageGroup.IsImage())
            {
                productGroup.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageGroup.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup", productGroup.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imageGroup.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup/Thumb", productGroup.ImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }
            #endregion

            await _productRepository.AddProductGroup(productGroup);
            await _productRepository.SaveChange();

            return CreateProductGroupRresult.Sucess;
        }

        public async Task<EditProducGroupResult> EditProducGroup(EditProducGroupViewModel editGroup, IFormFile imageGroup)
        {
            var productGroup = await _productRepository.GetProductGroupById(editGroup.ProductGroupId);
            if (productGroup == null) return EditProducGroupResult.NotFound;

            if (await _productRepository.ChechUrlNameGroup(editGroup.UrlName, editGroup.ProductGroupId)) return EditProducGroupResult.IsExitUrlName;

            productGroup.GroupTitle = editGroup.GroupTitle;
            productGroup.UrlName = editGroup.UrlName;
            productGroup.CreateDate = DateTime.Now;


            #region AddImage
            if (imageGroup != null && imageGroup.IsImage())
            {
                if (productGroup.ImageName != "shopping-cart.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup", productGroup.ImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup/Thumb", productGroup.ImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                productGroup.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageGroup.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup", productGroup.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imageGroup.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup/Thumb", productGroup.ImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }

            #endregion
            _productRepository.UpdateProductGroup(productGroup);
            await _productRepository.SaveChange();
            return EditProducGroupResult.Sucess;
        }

        public async Task<EditProducGroupViewModel> GetEditProducGroup(long GroupId)
        {
            var productGroup = await _productRepository.GetProductGroupById(GroupId);
            if (productGroup != null)
            {
                return new EditProducGroupViewModel
                {

                    GroupTitle = productGroup.GroupTitle,
                    ImageName = productGroup.ImageName,
                    ProductGroupId = productGroup.Id,
                    UrlName = productGroup.UrlName,
                    ParentId = productGroup.ParentId

                };

            }
            return null;
        }
        #endregion


        #region Product
        public async Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filter)
        {
            return await _productRepository.FilterProduct(filter);
        }
        public async Task<List<Product>> GetAllProduct()
        {
            return await _productRepository.GetAllProduct();
        }
        public async Task<List<SelectListItem>> GetGroupManageProduct()
        {
            return await _productRepository.GetGroupManageProduct();
        }

        public async Task<List<SelectListItem>> GetSubGroupManageProduct(long groupId)
        {
            return await _productRepository.GetSubGroupManageProduct(groupId);
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductViewModel create, IFormFile imgProduct)
        {
            var sanitizer = new HtmlSanitizer();
            create.Description = sanitizer.Sanitize(create.Description);
            var product = new Product
            {
                Name = create.Name,
                TitleInBrowser = create.TitleInBrowser,
                ShortDescription = create.ShortDescription,
                Description = create.Description,
                Price = create.Price,
                Count=create.Count,
                DicPercent=create.DicPercent,
                IsActive = create.IsActive,
                GroupId = create.GroupId,
                SubGroupId = create.SubGroupId

            };
            #region AddImage
            product.ImageName = "shopping-cart.png";
            if (imgProduct != null && imgProduct.IsImage())
            {
                product.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgProduct.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/Product", product.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgProduct.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/Product/Thumb", product.ImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }

            #endregion
            await _productRepository.AddProduct(product);
            await _productRepository.SaveChange();
            return CreateProductResult.Success;

        }

        public async Task<EditProductViewModel> GetEditProduct(long productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product != null)
            {
                return new EditProductViewModel()
                {
                    ProductId = productId,
                    Name = product.Name,
                    TitleInBrowser = product.TitleInBrowser,
                    ShortDescription = product.ShortDescription,
                    Description = product.Description,
                    Price = product.Price,
                    Count=product.Count,
                    DicPercent=product.DicPercent,
                    ImageName = product.ImageName,
                    IsActive = product.IsActive,
                    GroupId = product.GroupId,
                    SubGroupId = product.SubGroupId

                };
            }
            return null;
        }

        public async Task<EditProductReult> EditProduct(EditProductViewModel editProduct, IFormFile imgProduct)
        {
            var product = await _productRepository.GetProductById(editProduct.ProductId);
            if (product == null) return EditProductReult.NotFound;

            var sanitizer = new HtmlSanitizer();
            editProduct.Description = sanitizer.Sanitize(editProduct.Description);

            product.Name = editProduct.Name;
            product.TitleInBrowser = editProduct.TitleInBrowser;
            product.ShortDescription = editProduct.ShortDescription;
            product.Description = editProduct.Description;
            product.Price = editProduct.Price;
            product.Count = editProduct.Count;
            product.DicPercent = editProduct.DicPercent;
            product.IsActive = editProduct.IsActive;
            product.GroupId = editProduct.GroupId;
            product.SubGroupId = editProduct.SubGroupId;
            product.CreateDate = DateTime.Now;

            #region AddImage
            if (imgProduct != null && imgProduct.IsImage())
            {
                if (product.ImageName != "shopping-cart.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/Product", product.ImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/Product/Thumb", product.ImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                product.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgProduct.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup", product.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgProduct.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ImageGroup/Thumb", product.ImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }

            #endregion
            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChange();

            return EditProductReult.Success;

        }

        public async Task<bool> DeleteProduct(long productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            return await _productRepository.RecoverProduct(productId);
        }
     
        public async Task<List<ProductListItemViewModel>> LatestProducts()
        {
            return await _productRepository.LatestProducts();
        }
        public async Task<List<ProductListItemViewModel>> ShowPopularProduct()
        {
            return await _productRepository.ShowPopularProduct();
        }
        public async Task<ProductDetailViewModel> ProductDetails(long productId)
        {
            return await _productRepository.ProductDetails(productId);
        }

        public async Task<List<ProductListItemViewModel>> RelatedProducts(string urlName, long productId)
        {
            return await _productRepository.RelatedProducts(urlName, productId);
        }

        #endregion

        #region ProductGallery
        public async Task<bool> AddProductGallery(long productId, List<IFormFile> imgProducts)
        {
            if (!await _productRepository.CheckProduct(productId))
            {
                return false;
            }
            if (imgProducts != null && imgProducts.Any())
            {
                var productGallery = new List<ProductGallery>();
                foreach (var img in imgProducts)
                {
                    if (img.IsImage())
                    {

                        var imageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(img.FileName);
                        string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ProductGallery", imageName);

                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            img.CopyTo(stream);
                        }
                        string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ProductGallery/Thumb", imageName);

                        var imagResize = new ImageConvertor();

                        imagResize.Image_resize(ImagePath, ImagePathThumb, 150);

                        productGallery.Add(new ProductGallery
                        {
                            ImageName = imageName,
                            ProductId = productId
                        });
                    }
                }
                await _productRepository.AddproductGalleries(productGallery);


            }
            return true;
        }

        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            return await _productRepository.GetAllProductGalleries(productId);
        }

        public async Task DeleteImageGallery(long galleryId)
        {
            var gallery = await _productRepository.GetProductGalleryById(galleryId);
            if (gallery != null)
            {
                string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ProductGallery", gallery.ImageName);
                if (File.Exists(deleteimagePath))
                {
                    File.Delete(deleteimagePath);
                }

                string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageProduct/ProductGallery/Thumb", gallery.ImageName);
                if (File.Exists(deletethumbPath))
                {
                    File.Delete(deletethumbPath);
                }
                await _productRepository.DeleteProductGallery(galleryId);
            }



        }

        #endregion

        #region ProductFeature
        public async Task<ProductFeatureResult> CreateProductFeature(CreateProductFeatureViewModel createFeature)
        {
            if (!await _productRepository.CheckProduct(createFeature.ProductId))
            {
                return ProductFeatureResult.Notfound;
            }

            var feature = new ProductFeature
            {
                FeatureTitle = createFeature.FeatureTitle,
                FeatureValue = createFeature.FeatureValue,
                ProductId = createFeature.ProductId
            };
            await _productRepository.AddProductFeature(feature);
            await _productRepository.SaveChange();

            return ProductFeatureResult.Success;
        }

        public async Task<List<ProductFeature>> GetAllProductFeatures(long productId)
        {
            return await _productRepository.GetAllProductFeatures(productId);
        }

        public async Task DeleteFeature(long id)
        {
            await _productRepository.DeleteFeature(id);
        }
        #endregion

        #region ProductComment
        public async Task<FilterCommentViewModel> FilterCommentProduct(FilterCommentViewModel filter)
        {
            return await _productRepository.FilterCommentProduct(filter);
        }

        public async Task<CreateProductCommentResult> CreateProductComment(CreateProductCommentViewModel createComment, long userId)
        {
            var comment = new ProductComment
            {
                UserId = userId,
                 ProductId=createComment.ProductId,
                  Comment=createComment.Comment,
                  IsReadAdmin=false,
                  ParentId=createComment.ParentId
                                                   
            };

            await _productRepository.AddProductComment(comment);
            await _productRepository.SaveChange();

            return CreateProductCommentResult.Success;
           
        }


        public async Task<List<ProductComment>> GetProductComment(long productId)
        {
            return await _productRepository.GetProductComment(productId);
        }

        public async Task ReadAdmin(long commentId)
        {
            var comment = await _productRepository.GetCommentById(commentId);
            comment.IsReadAdmin = true;
            _productRepository.UpdateComment(comment);
            await _productRepository.SaveChange();
        }

        public async Task DeleteComment(long id)
        {
            await _productRepository.DeleteComment(id);
            await _productRepository.SaveChange();
        }
        #endregion

        #region ProductTag
        public async Task<CreateProductTagResult> CreateProductTag(CreateProductTagViewModel createTag)
        {
            if (!await _productRepository.CheckProduct(createTag.ProductId))
            {
                return CreateProductTagResult.NotFount;
            }
            var tag = new ProductTag
            {
                ProductId = createTag.ProductId,
                TagName = createTag.TagName
            };
            await _productRepository.AddProductTag(tag);
            await _productRepository.SaveChange();

            return CreateProductTagResult.Success;
        }

        public async Task<List<ProductTag>> GetAllProductTag(long productId)
        {
            return await _productRepository.GetAllProductTag(productId);
        }

        public async Task DeleteTag(long id)
        {
            await _productRepository.DeleteTag(id);
           await _productRepository.SaveChange();
        }

     




        #endregion
    }
}
