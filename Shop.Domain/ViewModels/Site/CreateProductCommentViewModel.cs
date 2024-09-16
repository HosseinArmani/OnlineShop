using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site
{
   public class CreateProductCommentViewModel
    {
        public long ProductId { get; set; }
        public long? ParentId { get; set; }
        public string Comment { get; set; }
    }

    public enum CreateProductCommentResult
    {
        Success
    }
}
