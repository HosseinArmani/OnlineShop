using Ganss.Xss;
using Microsoft.AspNetCore.Http;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Articles;
using Shop.Domain.ViewModels.Admin.Article;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        #region Article
        public async Task<List<Article>> ShowAllArticle()
        {
            return await _articleRepository.ShowAllArticle();
        }


        public async Task<CreateArticleResult> CreateArticle(CreateArticleViewModel createArticle, IFormFile imgArticle)
        {
            var sanitizer = new HtmlSanitizer();
            createArticle.Description = sanitizer.Sanitize(createArticle.Description);
            var article = new Article
            {
                ArticleTitle = createArticle.ArticleTitle,
                TitleInBrowser = createArticle.TitleInBrowser,
                Description = createArticle.Description,
                ShortDescription = createArticle.ShortDescription,
                IsActive = createArticle.IsActive,

            };
            #region AddImage
            article.ImageName = "shopping-cart.png";
            if (imgArticle != null && imgArticle.IsImage())
            {
                article.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgArticle.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageArticle", article.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgArticle.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageArticle/Thumb", article.ImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }

            #endregion
            await _articleRepository.AddArticle(article);
            await _articleRepository.SaveChange();
            return CreateArticleResult.Success;
        }



        public async Task<EditArticleResult> EditArticle(EditArticleViewModel editArticle, IFormFile imgArticle)
        {
          
            var article = await _articleRepository.GetArticleById(editArticle.ArticleId);
            if (article == null) return EditArticleResult.NotFound;

            var sanitizer = new HtmlSanitizer();
            editArticle.Description = sanitizer.Sanitize(editArticle.Description);

            article.ArticleTitle = editArticle.ArticleTitle;
            article.TitleInBrowser = editArticle.TitleInBrowser;
            article.ShortDescription = editArticle.ShortDescription;
            article.Description = editArticle.Description;
            article.IsActive = editArticle.IsActive;
            article.CreateDate = DateTime.Now;
            

            #region AddImage
            if (imgArticle != null && imgArticle.IsImage())
            {
                if (article.ImageName != "shopping-cart.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageArticle", article.ImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageArticle/Thumb", article.ImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                article.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgArticle.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageArticle", article.ImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgArticle.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageArticle/Thumb", article.ImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }
            #endregion

            _articleRepository.UpdateArticle(article);
            await _articleRepository.SaveChange();

            return EditArticleResult.Success;

        }


        public async Task<EditArticleViewModel> GetEditArticle(long articleId)
        {
            var article = await _articleRepository.GetArticleById(articleId);
            return new EditArticleViewModel
            {
                ArticleId = article.Id,
                ArticleTitle = article.ArticleTitle,
                ShortDescription = article.ShortDescription,
                Description = article.Description,
                TitleInBrowser = article.TitleInBrowser,
                ImageName = article.ImageName,
                IsActive = article.IsActive,
               

            };

        }

    
        public async Task<List<ArticlelistItemViewModel>> LatestArticle()
        {
            return await _articleRepository.LatestArticle();
        }
        public async Task<List<ArticlelistItemViewModel>> LatestArticleSingle(long articleId)
        {
            return await _articleRepository.LatestArticleSingle(articleId);
        }

        public async Task<ArticleDetailViewModel> AticleDetails(long articleId)
        {
            return await _articleRepository.AticleDetails(articleId);
        }

        public async Task<FilterArticleViewModel> FilterArticle(FilterArticleViewModel filter)
        {
            return await _articleRepository.FilterArticle(filter);
        }
        #endregion

        #region ArticleTag
        public async Task<CreateArticleTagResult> CreateArticleTag(CreateArticleTagViewModel createArticleTag)
        {
            if (!await _articleRepository.CheckArticle(createArticleTag.ArticleId))
            {
                return CreateArticleTagResult.NotFount;
            }
            var articleTag = new ArticleTag
            {
                ArticleId = createArticleTag.ArticleId,
                TagName = createArticleTag.TagName
            };
            await _articleRepository.AddArticleTag(articleTag);
            await _articleRepository.SaveChange();
            return CreateArticleTagResult.Success;
        }

        public async Task<List<ArticleTag>> GetAllArticleTag(long articleId)
        {
            return await _articleRepository.GetAllArticleTag(articleId);
        }

        public async Task DeleteTag(long id)
        {
            await _articleRepository.DeleteTag(id);
            await _articleRepository.SaveChange();
        }

       



        #endregion

    }
}
