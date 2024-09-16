using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Wallet;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositores
{


    public class WalletRepository : IWalletRepository
    {
        private readonly ShopDbContext _context;
        private readonly IUserRepository _userRepository;
        public WalletRepository(ShopDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task AddWallet(UserWallet userWallet)
        {
            await _context.userWallets.AddAsync(userWallet);

        }

        public async Task<UserWallet> GetByWalletUserId(long walletId)
        {
            return await _context.userWallets.AsQueryable()
                   .SingleOrDefaultAsync(w => w.Id == walletId);
        }


        public void UpdateWallet(UserWallet wallet)
        {
            _context.userWallets.Update(wallet);
        }
        public async  Task<int> BalanceWallet(string phoneNumber)
        {
            var userid = _userRepository.GetUserIdByPhoneNumber(phoneNumber);
            var deposit =await _context.userWallets.Where(w => w.UserId == userid && w.WalletType == WalletType.Variz && w.IsPay)
                .Select(w => w.Amount).ToListAsync();

            var Withdraw = await _context.userWallets.Where(w => w.UserId == userid && w.WalletType == WalletType.Bardasht)
                .Select(w => w.Amount).ToListAsync();

            return   (deposit.Sum() - Withdraw.Sum());

        }
        public async Task<int> BalanceWallet(long userId)
        {
            
            var deposit = await _context.userWallets.Where(w => w.UserId == userId && w.WalletType == WalletType.Variz && w.IsPay)
                .Select(w => w.Amount).ToListAsync();

            var Withdraw = await _context.userWallets.Where(w => w.UserId == userId && w.WalletType == WalletType.Bardasht && w.IsPay)
                .Select(w => w.Amount).ToListAsync();

            return (deposit.Sum() - Withdraw.Sum());
        }
        public async Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter)
        {
            var query = _context.userWallets.AsQueryable();

            if(filter.UserId !=0 && filter.UserId != null)
            {
                query = query.Where(w => w.UserId == filter.UserId && w.IsPay);
            }

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);

            var allData = await query.Paging(pager).ToListAsync();

            return filter.SetPaging(pager).SetWallet(allData);
        }
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

     
    }
}
