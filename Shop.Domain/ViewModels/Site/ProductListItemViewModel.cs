using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site
{
   public class ProductListItemViewModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
        public int? DicPercent { get; set; }

    }
}
