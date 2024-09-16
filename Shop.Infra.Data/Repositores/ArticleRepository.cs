using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Articles;
using Shop.Domain.ViewModels.Admin.Article;
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
    public class ArticleRepository : IArticleRepository
    {
        private readonly ShopDbContext _context;
        public ArticleRepository(ShopDbContext context)
        {
            _context = context;
        }


        #region Article
        public async Task<List<Article>> ShowAllArticle()
        {
            return await _context.Articles.AsQueryable().Where(c => c.IsActive).ToListAsync();
        }

        public async Task AddArticle(Article article)
        {
            await _context.Articles.AddAsync(article);
        }


        public async Task<bool> CheckArticle(long articleId)
        {
            return await _context.Articles.AsQueryable()
                 .AnyAsync(t => t.Id == articleId);
        }

        public async Task<Article> GetArticleById(long articleId)
        {
            return await _context.Articles.FindAsync(articleId);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

   
        public void UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
        }
        public async Task<List<ArticlelistItemViewModel>> LatestArticle()
        {
            var Latest = await _context.Articles.AsQueryable().Where(c => c.IsActive)
               .OrderByDescending(c => c.CreateDate)
               .Select(c => new ArticlelistItemViewModel
               {
                   ArticleId = c.Id,
                   ArticleImage = c.ImageName,
                   ArticleTitle = c.ArticleTitle,

               }).Take(12).ToListAsync();

            return Latest;
        }
        public async Task<List<ArticlelistItemViewModel>> LatestArticleSingle(long articleId)
        {
            var Latest=await _context.Articles.AsQueryable().Where(c=>c.IsActive && c.Id != articleId)
                 .OrderByDescending(c => c.CreateDate)
               .Select(c => new ArticlelistItemViewModel
               {
                   ArticleId = c.Id,
                   ArticleImage = c.ImageName,
                   ArticleTitle = c.ArticleTitle,

               }).Take(6).ToListAsync();

            return Latest;
        }

        public async Task<ArticleDetailViewModel> AticleDetails(long articleId)
        {
            return await _context.Articles
                .Include(c => c.ArticleTags)
                .AsQueryable()
                .Where(c => c.Id == articleId).Select(c => new ArticleDetailViewModel
                {
                    ArticleId = c.Id,
                    ArticleTitle = c.ArticleTitle,
                    TitleInBrowser = c.TitleInBrowser,
                    ShortDescription = c.ShortDescription,
                    ImageName = c.ImageName,
                    Description = c.Description,
                    CreateDate = c.CreateDate,
                    ArticleTags = c.ArticleTags.ToList()

                }).SingleOrDefaultAsync();
        }
        public async Task<FilterArticleViewModel> FilterArticle(FilterArticleViewModel filter)
        {
            var query = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(filter.ArticlTitle))
            {
                query = query.Where(c => EF.Functions.Like(c.ArticleTitle, $"%{filter.ArticlTitle}%"));
            }
            if (!string.IsNullOrEmpty(filter.TitleInBrowser))
            {
                query = query.Where(c => EF.Functions.Like(c.TitleInBrowser, $"%{filter.TitleInBrowser}%"));
            }

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();

            return filter.SetPaging(pager).SetArticle(allData);
        }



        #endregion
        #region ArticleTag

        public async Task AddArticleTag(ArticleTag articleTag)
        {
            await _context.ArticleTags.AddAsync(articleTag);
        }

        public async Task<List<ArticleTag>> GetAllArticleTag(long articleId)
        {
            return await _context.ArticleTags.AsQueryable()
                .Where(t => t.ArticleId == articleId && !t.IsDelete).ToListAsync();
        }

        public async Task DeleteTag(long id)
        {
            var tag = await _context.ArticleTags.FindAsync(id);
            if (tag != null)
            {

                _context.ArticleTags.Remove(tag);

            }
        }

    



        #endregion
    }
}
