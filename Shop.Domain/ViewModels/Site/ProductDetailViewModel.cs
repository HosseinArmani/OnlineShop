using Shop.Domain.Models.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site
{
   public class ProductDetailViewModel
    {
        public long ProductId { get; set; } 
        public string Name { get; set; }
        public string TitleInBrowser { get; set; } 

        public string ShortDescription { get; set; }
      
        public string Description { get; set; }

        public int Price { get; set; }
        public int Count { get; set; }
        public int? DicPercent { get; set; }

        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public List<string> ImageGallery { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public List<ProductTag> ProductTags { get; set; }


    }
}
