using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Web.Extentions;
using Shop.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region ProductGroup
        [PermissionCheker(29)]
        public async Task<IActionResult> ShowProductGroup()
        {
            return View(await _productService.GetAllProductGroup());
        }
        [PermissionCheker(31)]
        [HttpGet]
        public IActionResult CreateProductGroup()
        {

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductGroup(CreateProductGroupViewModel create, IFormFile imageGroup, long? id)
        {
            if (ModelState.IsValid)
            {
                var ressult = await _productService.CreateProductGroup(create, imageGroup, id);
                switch (ressult)
                {
                    case CreateProductGroupRresult.IsExitUrlName:
                        TempData[ErrorMessage] = "عنوان یکتا تکراری می باشد ";
                        break;
                    case CreateProductGroupRresult.Sucess:
                        TempData[SuccessMessage] = "گروه ثبت شد ";
                        return RedirectToAction("ShowProductGroup");

                }
            }
            return View(create);
        }
        [PermissionCheker(32)]
        [HttpGet]
        public async Task<IActionResult> EditProductGroup(long id)
        {
            var group = await _productService.GetEditProducGroup(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductGroup(EditProducGroupViewModel editGroup, IFormFile imageGroup)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProducGroup(editGroup, imageGroup);
                switch (result)
                {
                    case EditProducGroupResult.IsExitUrlName:
                        TempData[ErrorMessage] = "عنوان یکتا تکراری می باشد ";
                        break;
                    case EditProducGroupResult.Sucess:
                        TempData[SuccessMessage] = "گروه ثبت شد ";
                        return RedirectToAction("ShowProductGroup");
                    case EditProducGroupResult.NotFound:
                        TempData[ErrorMessage] = "گروهی یافت نشد ";
                        break;

                }
            }
            return View(editGroup);
        }
        #endregion

        #region Product

        #region ShowProduct
        [PermissionCheker(9)]
        public async Task<IActionResult> ShowProduct(FilterProductViewModel filter)
        {
            return View(await _productService.FilterProduct(filter));
        }
        #endregion

        #region CreateProduct
        [PermissionCheker(10)]
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var groups = await _productService.GetGroupManageProduct();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var subGroups = await _productService.GetSubGroupManageProduct(long.Parse(groups.First().Value));
            ViewData["SubGroup"] = new SelectList(subGroups, "Value", "Text");

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel create, IFormFile imgProduct)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProduct(create, imgProduct);
                switch (result)
                {
                    case CreateProductResult.Success:
                        return RedirectToAction("ShowProduct");


                }
            }
            return View(create);
        }
        #endregion

        #region EditProduct
        [PermissionCheker(11)]
        [HttpGet]
        public async Task<IActionResult> EditProduct(long ProductId)
        {
            var product = await _productService.GetEditProduct(ProductId);

            var groups = await _productService.GetGroupManageProduct();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", product.GroupId);

            List<SelectListItem> subgroups = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "انتخاب کنید", Value = "" }
            };
            subgroups.AddRange(await _productService.GetSubGroupManageProduct(product.GroupId));
            string selectedSubGroup = null;
            if (product.SubGroupId != null)
            {
                selectedSubGroup = product.SubGroupId.ToString();
            }

            ViewData["SubGroup"] = new SelectList(subgroups, "Value", "Text", selectedSubGroup);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductViewModel editProduct, IFormFile imgProduct)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProduct(editProduct, imgProduct);
                switch (result)
                {
                    case EditProductReult.NotFound:
                        break;
                    case EditProductReult.Success:
                        return RedirectToAction("ShowProduct");


                }
            }
            return View(editProduct);
        }
        #endregion

        #region GetSubGroups-Json
        public async Task<IActionResult> GetSubGroups(long id)
        {
            List<SelectListItem> List = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value=""}
            };


            List.AddRange(await _productService.GetSubGroupManageProduct(id));
            return Json(new SelectList(List, "Value", "Text"));
        }
        #endregion

        #region DeleteProduct & RecoverProduct
        [PermissionCheker(12)]
        public async Task<IActionResult> DeleteProduct(long productId)
        {
            var result = await _productService.DeleteProduct(productId);
            if (result)
            {
                return RedirectToAction("ShowProduct");
            }
            return RedirectToAction("ShowProduct");
        }
        [PermissionCheker(13)]
        public async Task<IActionResult> RecoverProduct(long productId)
        {
            var result = await _productService.RecoverProduct(productId);
            if (result)
            {
                return RedirectToAction("ShowProduct");
            }
            return RedirectToAction("ShowProduct");
        }
        #endregion


        #endregion

        #region ProductGallery
        public IActionResult ProductGallery(long productId)
        {
            ViewBag.ProductId = productId;
            return View();

        }
        public async Task<IActionResult> AddImageToProduct(List<IFormFile> imgProducts, long productId)
        {
            var result = await _productService.AddProductGallery(productId, imgProducts);
            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }
        public async Task<IActionResult> ShowProductGallery(long ProductId)
        {
            var gallery = await _productService.GetAllProductGalleries(ProductId);
            if (gallery == null)
            {
                return null;
            }
            return View(gallery);
        }
        public async Task<IActionResult> DeleteProductGallery(long gallerId)
        {
            await _productService.DeleteImageGallery(gallerId);
            return RedirectToAction("ShowProduct");
        }
        #endregion

        #region ProductFeature
        public async Task<IActionResult> ShowProductFeature(long ProductId)
        {
            
            return View(await _productService.GetAllProductFeatures(ProductId));
        }

        [HttpGet] 
        public IActionResult CreateProductFeature(long ProductId)
        {
            var feature = new CreateProductFeatureViewModel
            {
                ProductId = ProductId
            };
            return View(feature);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductFeature(CreateProductFeatureViewModel createFeature)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductFeature(createFeature);
                switch (result)
                {
                    case ProductFeatureResult.Notfound:
                        TempData[ErrorMessage] = "این محصول وجود ندارد";
                        break;
                    case ProductFeatureResult.Success:

                        TempData[SuccessMessage] = "ویژگی محصول با موفقیت ثبت شد";
                        return RedirectToAction("ShowProduct");
                    
                }
            }
            
          
            return View(createFeature);
        }
        public async Task<IActionResult> DeleteFeature(long featurId)
        {
            await _productService.DeleteFeature(featurId);
            return RedirectToAction("ShowProduct");
        }
        #endregion

        #region ProductTag
        public async Task<IActionResult> ShowProductTag(long ProductId)
        {
            return View(await _productService.GetAllProductTag(ProductId));
        }
        [HttpGet]
        public IActionResult CreateProductTag(long ProductId)
        {
            var tag = new CreateProductTagViewModel()
            {
                ProductId = ProductId
            };

            return View(tag);
        }
        public async Task<IActionResult> CreateProductTag(CreateProductTagViewModel createTag)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductTag(createTag);

                switch (result)
                {
                    case CreateProductTagResult.NotFount:
                        TempData[ErrorMessage] = "این محصول وجود ندارد";
                        break;
                    case CreateProductTagResult.Success:

                        TempData[SuccessMessage] = "تگ محصول با موفقیت ثبت شد";
                        return RedirectToAction("ShowProduct"); ;
                    
                }
            }

            return View();
        }
        public async Task<IActionResult> DeleteTag(long tagId)
        {
            await _productService.DeleteTag(tagId);

            return RedirectToAction("ShowProduct");
        }


        #endregion
        public  async Task<IActionResult> ShowComment(FilterCommentViewModel filter)
        {
           
            return View(await _productService.FilterCommentProduct(filter));
        }
  
        public async Task<IActionResult> ReadAdminComment(long commentId)
        {
            await _productService.ReadAdmin(commentId);

            return RedirectToAction("ShowComment"); ;
        }
        public async Task<IActionResult> DeleteComment(long commentId)
        {
            await _productService.DeleteComment(commentId);

            return RedirectToAction("ShowComment");
        }

    }
}
