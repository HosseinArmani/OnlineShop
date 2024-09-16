using Shop.Domain.Models.Articles;
using Shop.Domain.ViewModels.Admin.Article;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
   public interface IArticleRepository
    {
        #region Article
        Task<List<Article>> ShowAllArticle();
        Task AddArticle(Article article);
        Task<Article> GetArticleById(long articleId);
        void UpdateArticle(Article article);        
        Task<bool> CheckArticle(long articleId);
        Task<List<ArticlelistItemViewModel>> LatestArticle();
        Task<List<ArticlelistItemViewModel>> LatestArticleSingle(long articleId);
        Task<ArticleDetailViewModel> AticleDetails(long articleId);
        Task<FilterArticleViewModel> FilterArticle(FilterArticleViewModel filter);
        #endregion
        #region ArticleTag
        Task AddArticleTag(ArticleTag articleTag);
        Task<List<ArticleTag>> GetAllArticleTag(long articleId);
        Task DeleteTag(long id);
        #endregion
        Task SaveChange();
    }
}
