using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Account;
using Shop.Domain.ViewModels.Paging;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositores
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopDbContext _context;
        public UserRepository(ShopDbContext context)
        {
            _context = context;
        }
        #region Account
        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.AsQueryable()
                .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

        }
        public async Task<User> GetUserByActiveCode(string activeCode)
        {
            return await _context.Users.AsQueryable()
               .SingleOrDefaultAsync(u => u.ActiveCode == activeCode);
        }


        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(long userid)
        {
            return await _context.Users.AsQueryable().SingleOrDefaultAsync(u => u.Id == userid);
        }

        public long GetUserIdByPhoneNumber(string phoneNumber)
        {
            return  _context.Users.Single(u => u.PhoneNumber == phoneNumber).Id;
        }
        public async Task<Address> GetAddresByUserId(long userId)
        {
            return await _context.Addresses.SingleOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<Address> GetAddressById(long addressId)
        {
            return await _context.Addresses.AsQueryable().SingleOrDefaultAsync(a => a.Id == addressId);
        }


        public async Task AddAddress(Address address)
        {
            await _context.Addresses.AddAsync(address);
        }

        public void UpdateAddress(Address address)
        {
            _context.Addresses.Update(address);
        }
   
        #endregion
        #region Admin


        public async Task<FilterUserViewModel> FilterUser(FilterUserViewModel filter)
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(u => u.PhoneNumber == filter.PhoneNumber);
            }

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);

            var allData = await query.Paging(pager).ToListAsync();

            return filter.SetPaging(pager).SetUser(allData);

        }

        public async Task<EditUserForAdminViewModel> GetEditUserForAdmin(long userId)
        {
            return await _context.Users.AsQueryable()
                .Where(u => u.Id == userId).Select(u => new EditUserForAdminViewModel()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    SelectedRoles = u.UserRoles.Select(u => u.RoleId).ToList()

                }).SingleOrDefaultAsync();
        }

        public async Task AddRoleToUser(List<long> RoleIds, long userId)
        {

            foreach (var roleid in RoleIds)
            {
                await _context.AddAsync(new UserRoles()
                {
                    RoleId = roleid,
                    UserId = userId
                });
            }
         
  

        }

        public async Task<long> AddUserForAdmin(User user)
        {
            await _context.Users.AddAsync(user);
          await  _context.SaveChangesAsync();
            return  user.Id;

        }

        public async Task RemoveAllRoleSelected(long userId)
        {
           var RemoveAllRole= await _context.UserRoles.Where(u => u.UserId == userId).ToListAsync();
            if (RemoveAllRole.Any())
            {
                _context.UserRoles.RemoveRange(RemoveAllRole);
            }

        }








        #endregion



    }
}
