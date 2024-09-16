using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;  
        }
        [HttpGet("ArticleDetail/{articleId}")]
        public async Task<IActionResult> ArticleDetail(long articleId)
        {
            TempData["LatestArticle"] = await _articleService.LatestArticleSingle(articleId);
            return View(await _articleService.AticleDetails(articleId));
        }
     
        public async Task<IActionResult> ArchiveArticle(FilterArticleViewModel filter)
        {
            filter.TakeEntity = 12;
            return View(await _articleService.FilterArticle(filter));
        }
    }
}
