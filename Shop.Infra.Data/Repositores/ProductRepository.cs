using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Production;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Site;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositores
{

    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;
        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }


        #region ProductGroup
        public async Task<List<ProductGroup>> GetAllProductGroup()
        {
            return await _context.ProductGroups.Include(g => g.ProductGroups).ToListAsync();
        }
        public async Task<bool> ChechUrlNameGroup(string urlName)
        {
            return await _context.ProductGroups.AsQueryable()
                .AnyAsync(c => c.UrlName == urlName);
        }
        public async Task AddProductGroup(ProductGroup group)
        {
            await _context.ProductGroups.AddAsync(group);
        }
        public void UpdateProductGroup(ProductGroup group)
        {
            _context.ProductGroups.Update(group);
        }
        public async Task<ProductGroup> GetProductGroupById(long groupId)
        {
            return await _context.ProductGroups.FindAsync(groupId);
        }

        public async Task<bool> ChechUrlNameGroup(string urlName, long groupId)
        {
            return await _context.ProductGroups.AsQueryable()
                .AnyAsync(c => c.UrlName == urlName && c.Id != groupId);
        }
        #endregion
        
        #region Product
        public async Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filter)
        {
            var query = _context.Products.Include(c => c.ProductGroup).AsQueryable();


            #region filter
            if (!string.IsNullOrEmpty(filter.ProductName))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.ProductName}%"));
            }
            if (!string.IsNullOrEmpty(filter.TitleInBrowser))
            {
                query = query.Where(c => EF.Functions.Like(c.TitleInBrowser, $"%{filter.TitleInBrowser}%"));
            }
            if (!string.IsNullOrEmpty(filter.FilterByGroup))
            {
                query = query.Where(s => s.ProductGroup.UrlName == filter.FilterByGroup);

            }
            if (filter.SelectedGroups != null && filter.SelectedGroups.Any())
            {
                foreach (var groupId in filter.SelectedGroups)
                {
                    query = query.Where(c => c.GroupId == groupId || c.SubGroupId == groupId);
                }
            }

            #endregion
            #region State
            switch (filter.ProductState)
            {
                case ProductState.All:
                    query = query.Where(c => !c.IsDelete);
                    break;
                case ProductState.IaActive:
                    query = query.Where(c => c.IsActive);
                    break;
                case ProductState.Delete:
                    query = query.Where(c => c.IsDelete);
                    break;

            }
            #endregion

            #region Order
            switch (filter.ProductOrder)
            {
                case ProductOrder.All:
                    break;
                case ProductOrder.ProductNews:
                    query = query.Where(c => c.IsActive).OrderByDescending(c => c.CreateDate);
                    break;
                case ProductOrder.ProductExp:
                    query = query.Where(c => c.IsActive).OrderByDescending(c => c.Price);
                    break;
                case ProductOrder.ProductInExp:
                    query = query.Where(c => c.IsActive).OrderBy(c => c.Price);
                    break;

            }
            #endregion

            #region product box
            switch (filter.ProductBox)
            {
                case ProductBox.Default:
                    break;
                case ProductBox.ItemBoxInSite:

                    var pagerBox = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
                    var allDataBox = await query.Paging(pagerBox).Select(c => new ProductListItemViewModel
                    {
                        ProductId = c.Id,
                        ProductImage = c.ImageName,
                        ProductName = c.Name,
                        Price = c.Price,
                        DicPercent = c.DicPercent,
                        IsActive = c.IsActive
                    }).ToListAsync();
                    return filter.SetPaging(pagerBox).SetProductsItem(allDataBox);
            }
            #endregion

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();

            return filter.SetPaging(pager).SetProducts(allData);

        }
        public async Task<List<Product>> GetAllProduct()
        {
           return await _context.Products.AsQueryable().Where(p => !p.IsDelete ).ToListAsync();
        }
        public async Task<List<SelectListItem>> GetGroupManageProduct()
        {
            return await _context.ProductGroups.Where(g => g.ParentId == null).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.Id.ToString()

            }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetSubGroupManageProduct(long groupId)
        {
            return await _context.ProductGroups.Where(g => g.ParentId == groupId).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.Id.ToString()

            }).ToListAsync();
        }
        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }
        public async Task<Product> GetProductById(long productId)
        {
            return await _context.Products.FindAsync(productId);
        }
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
        public async Task<bool> DeleteProduct(long productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product != null)
            {
                product.IsDelete = true;
                UpdateProduct(product);
                await SaveChange();
            }
            return false;
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product != null)
            {
                product.IsDelete = false;
                UpdateProduct(product);
                await SaveChange();
            }
            return false;
        }
   
        public async Task<List<ProductListItemViewModel>> ShowPopularProduct()
        {
            var popularProduct = await _context.Products.Include(p => p.OrderDetails).AsQueryable()
                .Where(d=>!d.IsDelete && d.OrderDetails.Any()).OrderByDescending(d => d.OrderDetails.Count)
                .Select(c => new ProductListItemViewModel
                {
                    ProductId = c.Id,
                    ProductName = c.Name,
                    ProductImage = c.ImageName,
                    Price = c.Price,
                    Count=c.Count,
                    IsActive = c.IsActive,
                    DicPercent = c.DicPercent


                }).Take(8).ToListAsync();
            return popularProduct;
        }
        public async Task<List<ProductListItemViewModel>> LatestProducts()
        {
            var Latest = await _context.Products.AsQueryable().Where(c=>!c.IsDelete)
                .OrderByDescending(c => c.CreateDate)
                .Select(c => new ProductListItemViewModel
                {
                    ProductId = c.Id,
                    ProductName = c.Name,
                    ProductImage = c.ImageName,
                    Price = c.Price,
                    IsActive = c.IsActive,
                    DicPercent = c.DicPercent

                }).Take(9).ToListAsync();

            return Latest;
        }
        public async Task<ProductDetailViewModel> ProductDetails(long productId)
        {
            return await _context.Products
                .Include(c => c.productFeatures)
                .Include(c => c.productGalleries)
                .Include(c => c.ProductGroup)
                .Include(c=>c.ProductTags)
                .AsQueryable()
                .Where(c => c.Id == productId).Select(c => new ProductDetailViewModel
                {
                    Name = c.Name,
                    TitleInBrowser = c.TitleInBrowser,
                    ShortDescription = c.ShortDescription,
                    ProductId = c.Id,
                    Price = c.Price,
                    Count=c.Count,
                    ImageName = c.ImageName,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    ProductGroup = c.ProductGroup,
                    DicPercent = c.DicPercent,
                    ImageGallery = c.productGalleries.Where(c => !c.IsDelete).Select(c => c.ImageName).ToList(),
                    ProductFeatures = c.productFeatures.ToList(),
                    ProductTags=c.ProductTags.ToList()

                }).SingleOrDefaultAsync();
        }
        public async Task<List<ProductListItemViewModel>> RelatedProducts(string urlName,long productId)
        {
            var related = await _context.Products.AsQueryable()
                 .Where(p=>p.ProductGroup.UrlName==urlName&&p.Id!=productId)
                 .Select(c => new ProductListItemViewModel
                 {
                     ProductId = c.Id,
                     ProductName = c.Name,
                     ProductImage = c.ImageName,
                     Price = c.Price,
                     IsActive = c.IsActive,
                     DicPercent = c.DicPercent

                 }).Take(6).ToListAsync();

            return related;

           
        }
        #endregion

        #region ProductGallery
        public async Task AddproductGalleries(List<ProductGallery> productGalleries)
        {
            await _context.productGalleries.AddRangeAsync(productGalleries);
            await SaveChange();
        }
        public async Task<bool> CheckProduct(long productId)
        {
            return await _context.Products.AsQueryable()
                .AnyAsync(p => p.Id == productId);
        }
        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            return await _context.productGalleries.AsQueryable()
                   .Where(g => g.ProductId == productId && !g.IsDelete).ToListAsync();
        }
        public async Task<ProductGallery> GetProductGalleryById(long id)
        {
            return await _context.productGalleries.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == id);
        }
        public async Task DeleteProductGallery(long id)
        {
            var gallery = await _context.productGalleries.AsQueryable()
                 .SingleOrDefaultAsync(c => c.Id == id);
            if (gallery != null)
            {
                gallery.IsDelete = true;
                _context.productGalleries.Update(gallery);
                await SaveChange();
            }

        }
        #endregion

        #region ProductFeature
        public async Task AddProductFeature(ProductFeature product)
        {
            await _context.ProductFeatures.AddAsync(product);
        }
        public async Task<List<ProductFeature>> GetAllProductFeatures(long productId)
        {
            return await _context.ProductFeatures.AsQueryable()
                 .Where(f => f.ProductId == productId && !f.IsDelete).ToListAsync();
        }
        public async Task DeleteFeature(long id)
        {
            var feature = await _context.ProductFeatures.FindAsync(id);
            if (feature != null)
            {
                feature.IsDelete = true;
                _context.ProductFeatures.Update(feature);
                await SaveChange();
            }
        }
        #endregion

        #region ProductComment
        public async Task<FilterCommentViewModel> FilterCommentProduct(FilterCommentViewModel filter)
        {
            var query = _context.ProductComments.OrderByDescending(c => c.CreateDate).AsQueryable();

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();

            return filter.SetPaging(pager).SetProductComments(allData);
        }

        public async Task AddProductComment(ProductComment productComment)
        {
            await _context.ProductComments.AddAsync(productComment);
        }

        public async Task<List<ProductComment>>GetProductComment(long productId)
        {
            
               return await _context.ProductComments.Include(c => c.User).Where(c => !c.IsDelete && c.IsReadAdmin && c.ProductId == productId)
                    .OrderByDescending(c => c.CreateDate).ToListAsync();

        }
        #endregion

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddProductTag(ProductTag tag)
        {
            await _context.ProductTags.AddAsync(tag);
        }

        public async Task<List<ProductTag>> GetAllProductTag(long productId)
        {
            return await _context.ProductTags.AsQueryable()
                .Where(t => t.ProductId == productId && !t.IsDelete).ToListAsync();
        }

        public async Task DeleteTag(long id)
        {
            var tag = await _context.ProductTags.FindAsync(id);
            if (tag != null)
            {
                
                _context.ProductTags.Remove(tag);
                
            }
        }

        public void UpdateComment(ProductComment productComment)
        {
           
            _context.ProductComments.Update(productComment);
           
        }

        public async Task<ProductComment> GetCommentById(long commentId)
        {
            return await _context.ProductComments.FindAsync(commentId);
        }

        public async Task DeleteComment(long id)
        {
            var comment =await _context.ProductComments.FindAsync(id);
            if (comment != null)
            {
                _context.ProductComments.Remove(comment);
            }

        }
    }
}
