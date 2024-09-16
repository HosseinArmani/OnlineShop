using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Article;
using Shop.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ArticleController : BaseAdminController
    {
        private IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [PermissionCheker(14)]
        public async Task<IActionResult> ShowArticle(FilterArticleViewModel filter)
        {
            return View(await _articleService.FilterArticle(filter));
        }
        [PermissionCheker(15)]
        [HttpGet]
        public IActionResult CreateArticle()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleViewModel createArticle , IFormFile imgArticle)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleService.CreateArticle(createArticle, imgArticle);
                switch (result)
                {
                    case CreateArticleResult.Success:
                        TempData[SuccessMessage] = "مقاله شما با موفقیت ثبت شد";
                        return RedirectToAction("ShowArticle");
                }
            }
            return View(createArticle);
        }
        [PermissionCheker(16)]
        [HttpGet]
        public async Task<IActionResult> EditArticle(long articleId)
        {
            var article = await _articleService.GetEditArticle(articleId);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
        [HttpPost]
        public async Task<IActionResult> EditArticle(EditArticleViewModel editArticle, IFormFile imgArticle)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleService.EditArticle(editArticle,imgArticle);
                switch (result)
                {
                    case EditArticleResult.NotFound:
                        TempData[ErrorMessage] = "مقاله ای یافت نشد";
                        break;
                    case EditArticleResult.Success:
                        TempData[SuccessMessage] = "مقاله شما با موفقیت ویرایش شد";
                        return RedirectToAction("ShowArticle");
                
                }

            }
            return View(editArticle);
        }
        [HttpGet]
        public IActionResult CreateArticleTag(long articleId)
        {
            var tag = new CreateArticleTagViewModel
            {
                 ArticleId=articleId
            };
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticleTag(CreateArticleTagViewModel createArticleTag)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleService.CreateArticleTag(createArticleTag);
                switch (result)
                {
                    case CreateArticleTagResult.NotFount:
                        TempData[ErrorMessage] = "مقاله ای یافت نشد";
                        break;
                    case CreateArticleTagResult.Success:
                        TempData[SuccessMessage] = "مقاله شما با موفقیت ثبت شد";
                        return RedirectToAction("ShowArticle");
                        
                    
                }
            }
            return View(createArticleTag);
        }
        public async Task<IActionResult> ShowArticleTag(long articleId)
        {
            return View(await _articleService.GetAllArticleTag(articleId));
        }
        public async Task<IActionResult> DeleteTag(long tagId)
        {
            await _articleService.DeleteTag(tagId);

            return RedirectToAction("ShowArticle");
        }
    }
}
