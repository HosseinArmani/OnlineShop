using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
  public interface IUserRepository
    {
        #region Account
        Task AddUser(User user);
        void UpdateUser(User user);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> GetUserByActiveCode(string activeCode);
        Task<User> GetUserById(long userid);
        long GetUserIdByPhoneNumber(string phoneNumber);
        Task<Address> GetAddressById(long addressId);
        Task AddAddress(Address address);
        void UpdateAddress(Address address);
        Task<Address> GetAddresByUserId(long userId);
      


        Task SaveChange();
        #endregion

        #region Admin
        Task<long> AddUserForAdmin(User user); 
        Task<FilterUserViewModel> FilterUser(FilterUserViewModel filter);
        Task<EditUserForAdminViewModel> GetEditUserForAdmin(long userId);
        Task AddRoleToUser(List<long> RoleIds, long userId);
        Task RemoveAllRoleSelected(long userId);
    
        #endregion
    }
}
