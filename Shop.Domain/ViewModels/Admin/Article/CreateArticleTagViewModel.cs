using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Article
{
  public class CreateArticleTagViewModel
    {
        public long ArticleId { get; set; }
        [Display(Name = "برچسب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string TagName { get; set; }

    }
    public enum CreateArticleTagResult
    {
        NotFount,
        Success
    }
}
