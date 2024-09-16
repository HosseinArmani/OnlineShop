using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Production
{
   public class ProductComment:BaseEntitity
    {
        public long ProductId { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
        public bool IsReadAdmin { get; set; }
        public long? ParentId { get; set; }


        #region Nav
        [ForeignKey("ParentId")]
        public ICollection<ProductComment> ProductComments { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        #endregion

    }
}
