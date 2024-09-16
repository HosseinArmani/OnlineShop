using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.Models.Orders;
using Shop.Web.Extentions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Web.ViewComponents
{
    #region Site Header
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly IUserServices _userServices;
        private readonly ISiteService _siteService;
        private readonly IOrderService _orderService;
        public SiteHeaderViewComponent(IUserServices userServices, ISiteService siteService, IOrderService orderService )
        {
            _userServices = userServices;
            _siteService = siteService;
           
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _userServices.GetUserByPhoneNumbers(User.Identity.Name);
                ViewBag.FavoriteCount = await _siteService.UserFavoriteCount(User.GetUserId());
              
            }
            return View("SiteHeader");
        }

    }
    #endregion

    #region Site Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }

    }
    #endregion

    #region Slider-Home
    public class SiteSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
       
        public SiteSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = await _siteService.GetAllSlider();
            return View("SiteSlider", slider);
        }

    }
    #endregion

    #region ProductGroup
    public class ProductGroupViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductGroupViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var group = await _productService.GetAllProductGroup();
            return View("ProductGroup", group);
        }

    }
    #endregion

    #region SideBarGroup
    public class SideBarGroupViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public SideBarGroupViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var group = await _productService.GetAllProductGroup();
            return View("SideBarGroup", group);
        }

    }
    #endregion

    #region SideBarMobile
    public class SideBarMobileViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public SideBarMobileViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var group = await _productService.GetAllProductGroup();
            return View("SideBarMobile", group);
        }

    }
    #endregion


    #region Popular
    public class PopularProductViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public PopularProductViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var popular = await _productService.ShowPopularProduct();
            return View("PopularProduct", popular);
        }

    }
    #endregion
    #region LatestProducts
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Latest = await _productService.LatestProducts();
            return View("LatestProducts", Latest);
        }

    }
    #endregion
 
    #region ShowCart
    public class ShowCartViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
       

        public ShowCartViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
           
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
               var cart = await _orderService.GetShowBasket(User.GetUserId());
            


            return View("ShowCart", cart);
        }

    }
    #endregion

    #region LatestArticle
    public class LatestArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;

        public LatestArticlesViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Latest = await _articleService.LatestArticle();
            return View("LatestArticles", Latest);
        }

    }
    #endregion

    #region ProductComment
    public class ProductCommentViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductCommentViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(long productId)
        {
            var comment =await _productService.GetProductComment(productId);

            return View("ProductComment", comment);
        }

    }
    #endregion

}
