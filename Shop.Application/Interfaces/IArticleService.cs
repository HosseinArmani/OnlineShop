using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Articles;
using Shop.Domain.ViewModels.Admin.Article;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
   public interface IArticleService
    {
        #region Article
        Task<List<Article>> ShowAllArticle();
        Task<CreateArticleResult> CreateArticle(CreateArticleViewModel createArticle, IFormFile imgArticle);
        Task<EditArticleViewModel> GetEditArticle(long articleId);
        Task<EditArticleResult> EditArticle(EditArticleViewModel editArticle, IFormFile imgArticle);
        Task<List<ArticlelistItemViewModel>> LatestArticle();
        Task<List<ArticlelistItemViewModel>> LatestArticleSingle(long articleId);
        Task<ArticleDetailViewModel> AticleDetails(long articleId);
        Task<FilterArticleViewModel> FilterArticle(FilterArticleViewModel filter);

        #endregion
        #region ArticleTag
        Task<CreateArticleTagResult> CreateArticleTag(CreateArticleTagViewModel createArticleTag);
        Task<List<ArticleTag>> GetAllArticleTag(long articleId);
        Task DeleteTag(long id);
        #endregion

    }
}
