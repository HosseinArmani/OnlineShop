using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Wallet
{
  public  class FilterWalletViewModel: BasePaging
    {
        public long? UserId { get; set; } 
        public List<UserWallet> UserWallets { get; set; }

        #region methods
        public FilterWalletViewModel SetWallet(List<UserWallet> userWallets)
        {
            this.UserWallets = userWallets;
            return this;
        }

        public FilterWalletViewModel SetPaging(BasePaging basePaging)
        {
            this.PageId = basePaging.PageId;
            this.PageCount = basePaging.PageCount;
            this.AllEntityCount = basePaging.AllEntityCount;
            this.CountForShowAfterAndBefor = basePaging.CountForShowAfterAndBefor;
            this.EndPage = basePaging.EndPage;
            this.SkipEntitiy = basePaging.SkipEntitiy;
            this.StartPage = basePaging.StartPage;
            this.TakeEntity = basePaging.TakeEntity;

            return this;

        }
        #endregion
    }
}
