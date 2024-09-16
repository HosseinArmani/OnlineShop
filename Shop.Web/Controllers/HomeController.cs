using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Domain.ViewModels.Site;
using Shop.Domain.ViewModels.Wallet;
using Shop.Web.Extentions;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Shop.Web.Extentions.SiteMapGenarator;

namespace Shop.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        string authority;
        private readonly IWalletService _walletService;

        private readonly IProductService _productService;

        private readonly IOrderService _orderService;
        private readonly IArticleService _articleService;
        private readonly IMemoryCache _memoryCache;
        
        public HomeController(IWalletService walletService, IProductService productService, IOrderService orderService, IArticleService articleService,IMemoryCache memoryCache)
        {
            _walletService = walletService;
            _productService = productService;
            _orderService = orderService;
            _articleService = articleService;
            _memoryCache = memoryCache;
          

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        #region VerifyPayment

        [HttpGet("verify-Payment/{id}")]
        public async Task<IActionResult> VerifyPayment(long id)
        {

            // string authorityverify;

            try
            {
                VerifyParameters parameters = new VerifyParameters();


                if (HttpContext.Request.Query["Authority"] != "")
                {
                    authority = HttpContext.Request.Query["Authority"];
                }
                var wallet = await _walletService.GetByWalletUserId(id);

                parameters.authority = authority;
                parameters.amount = wallet.Amount.ToString();
                parameters.merchant_id = "b7531f90-420b-44a3-acf9-3204cc0d7015";


                var client = new RestClient(URLs.verifyUrl);
                Method method = Method.Post;
                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.ExecuteAsync(request);


                JObject jodata = JObject.Parse(response.Result.Content);

                string data = jodata["data"].ToString();

                JObject jo = JObject.Parse(response.Result.Content);

                string errors = jo["errors"].ToString();

                if (data != "[]")
                {
                    string refid = jodata["data"]["ref_id"].ToString();


                    ViewBag.code = refid;
                    ViewBag.Success = true;
                    await _walletService.UpdateWalletForCharge(wallet);


                    return View();
                }
                else if (errors != "[]")
                {

                    string errorscode = jo["errors"]["code"].ToString();

                    return BadRequest($"error code {errorscode}");

                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return NotFound();
        }

        #endregion


        #region VerifyPaymentOrder
        [HttpGet("verify-PaymentOrder/{id}")]
        public async Task<IActionResult> VerifyPaymentOrder(long id)
        {

            // string authorityverify;

            try
            {
                VerifyParameters parameters = new VerifyParameters();


                if (HttpContext.Request.Query["Authority"] != "")
                {
                    authority = HttpContext.Request.Query["Authority"];
                }
                var order = await _orderService.GetOrderById(id);

                var transportation = await _orderService.GetTransportations();
                string orderSum = order.OrderSum.ToString();
                string ordersumTransportation = (order.OrderSum + transportation.Amount).ToString();

                parameters.authority = authority;
                parameters.amount = (order.OrderSum <= transportation.minBuy) ? ordersumTransportation : orderSum;
                parameters.merchant_id = "b7531f90-420b-44a3-acf9-3204cc0d7015";


                var client = new RestClient(URLs.verifyUrl);
                Method method = Method.Post;
                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.ExecuteAsync(request);


                JObject jodata = JObject.Parse(response.Result.Content);

                string data = jodata["data"].ToString();

                JObject jo = JObject.Parse(response.Result.Content);

                string errors = jo["errors"].ToString();

                if (data != "[]")
                {
                    string refid = jodata["data"]["ref_id"].ToString();


                    ViewBag.code = refid;
                    ViewBag.Success = true;
                    await _orderService.ChangeIsFinalyOrder(order.Id);


                    return View();
                }
                else if (errors != "[]")
                {

                    string errorscode = jo["errors"]["code"].ToString();

                    return BadRequest($"error code {errorscode}");

                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return NotFound();
        }
        #endregion
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        [Route("sitemap.xml")]
        public async Task<ContentResult> CreateSiteMap()
        {
            _memoryCache.TryGetValue("sitemapDateTime", out DateTime cacheValue);
            if (DateTime.Now > cacheValue.AddDays(1))
            {

                _memoryCache.Set("sitemapDateTime", DateTime.Now, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

                List<SitemapNode> sitemapNodes = new List<SitemapNode>();

                sitemapNodes.Add(new SitemapNode { Loc = "https://tasland.ir", Priority = 1, Frequency = SiteMapFrequency.weekly });
                var product = await _productService.GetAllProduct();
                foreach (var item in product)
                {
                    sitemapNodes.Add(new SitemapNode { Loc = "https://tasland.ir/ProductDetaeil/" + item.Id, Priority = 0.8, LastModified = item.CreateDate, Frequency = SiteMapFrequency.weekly });
                }
                var productGroup = await _productService.GetAllProductGroup();
                foreach (var item in productGroup)
                {
                    sitemapNodes.Add(new SitemapNode { Loc = "https://tasland.ir/Product/ArchiveProduct?SelectedGroups=" + item.Id, Priority = 0.8, LastModified = item.CreateDate, Frequency = SiteMapFrequency.weekly });
                }
                var article = await _articleService.ShowAllArticle();
                foreach (var item in article)
                {
                    sitemapNodes.Add(new SitemapNode { Loc = "https://tasland.ir/ArticleDetail/" + item.Id, Priority = 0.8, LastModified = item.CreateDate, Frequency = SiteMapFrequency.weekly });
                }
                sitemapNodes.Add(new SitemapNode { Loc = "https://tasland.ir/Home/AboutUs", Priority = 0.7, Frequency = SiteMapFrequency.weekly });
                sitemapNodes.Add(new SitemapNode { Loc = "https://tasland.ir/Home/ContactUs", Priority = 0.7, Frequency = SiteMapFrequency.weekly });

                new SiteMapGenarator().CreateSitemapXml(sitemapNodes, "");
            }
            var sitemaoPath = Path.Combine("", "sitemap.xml");
            string output = await System.IO.File.ReadAllTextAsync(sitemaoPath);

            return new ContentResult
            {
                Content = output,
                 ContentType= "application/xml",
                  StatusCode=200

            };

        }



    }
}
