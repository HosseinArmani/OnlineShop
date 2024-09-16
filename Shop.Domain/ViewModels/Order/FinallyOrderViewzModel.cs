using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Order
{
   public class FinallyOrderViewzModel
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public bool IsPayment { get; set; }

    }
    public enum FinallyOrderResult
    {
        HasNotUser,
        Errore,
        Success,
        NotFound
    }
}
