using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
  public interface IWalletService
    {
        Task<long> ChargeWallet(long userId, ChargeWalletViewModel charge, string description);
        Task<UserWallet> GetByWalletUserId(long walletId);
        Task<bool> UpdateWalletForCharge(UserWallet wallet);
        Task<int> BalanceWallet(string phoneNumber);
        Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter);
    }
}
