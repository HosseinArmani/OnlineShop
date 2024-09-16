using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Product;
using Shop.Domain.ViewModels.Site;
using Shop.Web.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        public ProductController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        public async Task<IActionResult> ArchiveProduct(FilterProductViewModel filter)
        {
            filter.TakeEntity = 12;
            filter.ProductBox = ProductBox.ItemBoxInSite;
            ViewData["ProductGroup"] = await _productService.GetAllProductGroup();
            ViewData["SelectedGroups"] = filter.SelectedGroups;
            return View(await _productService.FilterProduct(filter));
        }
        [HttpGet("ProductDetaeil/{productId}")]
        public async Task<IActionResult> ProductDetail(long productId)
        {
            var detail = await _productService.ProductDetails(productId);
            TempData["RelatedProducts"] = await _productService.RelatedProducts(detail.ProductGroup.UrlName, productId);

            return View(detail);
        }
        [Authorize]
        public async Task<IActionResult> BuyProduct(long productId)
        {
            var orderId = await _orderService.AddOrder(User.GetUserId(), productId);
            return Redirect("/User/Basket/" + orderId);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateProductCommentViewModel createComment,IFormCollection form)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LeLlb8kAAAAAHvKI1wAArU2h3xgXyP53JHZs02C"; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "Sorry, please validate the reCAPTCHA";
                return View();
            }

            // go ahead and write code to validate username password against database

            if (!String.IsNullOrEmpty(createComment.Comment))
            {
                var res = await _productService.CreateProductComment(createComment, User.GetUserId());
                switch (res)
                {
                    case CreateProductCommentResult.Success:
                        TempData[SuccessMessage] = "نظر شما با موفقیت ثبت شد بعد از باز بینی نمایش داده می شود";
                        return RedirectToAction("ProductDetail", new { productId = createComment.ProductId });
                }
            }
            TempData[ErrorMessage] = "لطفا نظر خودتان را بنویسید";
            return RedirectToAction("ProductDetail", new { productId = createComment.ProductId });
        }
      
    }
}
