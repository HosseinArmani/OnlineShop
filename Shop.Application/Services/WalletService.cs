using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        public WalletService(IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

    

        public async Task<long> ChargeWallet(long userId, ChargeWalletViewModel charge, string description)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null) return 0;
            var wallet = new UserWallet
            {
                UserId = userId,
                Amount = charge.Amount,
                Description = description,
                IsPay = false,
                WalletType = WalletType.Variz
            };
            await _walletRepository.AddWallet(wallet);
            await _walletRepository.SaveChange();

            return wallet.Id;
        }

        public async Task<UserWallet> GetByWalletUserId(long walletId)
        {
          return await _walletRepository.GetByWalletUserId(walletId);
        }

        public async Task<bool> UpdateWalletForCharge(UserWallet wallet)
        {
            if (wallet != null)
            {
                wallet.IsPay = true;
                _walletRepository.UpdateWallet(wallet);
                await _walletRepository.SaveChange();
                return true;
            }
           
            return false;
        }

        public async Task<int> BalanceWallet(string phoneNumber)
        {
          return await _walletRepository.BalanceWallet(phoneNumber);
        }

        public async Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter)
        {
            return await _walletRepository.FilterWallet(filter);
        }
    }
}
