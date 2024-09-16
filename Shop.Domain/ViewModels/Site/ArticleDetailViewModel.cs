using Shop.Domain.Models.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site
{
   public class ArticleDetailViewModel
    {
        public long ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string TitleInBrowser { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }
    }
}
