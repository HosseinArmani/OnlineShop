using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
   public interface IWalletRepository
    {
        Task AddWallet(UserWallet userWallet);
        Task<UserWallet> GetByWalletUserId(long walletId);
        void UpdateWallet(UserWallet wallet);
        Task<int> BalanceWallet(string phoneNumber);
        Task<int> BalanceWallet(long userId);

        Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter);
        Task SaveChange();
    }
}
