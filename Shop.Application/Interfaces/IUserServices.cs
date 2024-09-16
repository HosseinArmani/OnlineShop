using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
  public  interface IUserServices
    {
        #region Account
        Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register);
        Task AgainActiveCode(RegisterUserViewModel register);
        Task<bool> CheckActiveCode(RegisterUserViewModel register);
        Task<LoginUserResult> LoginUser(LoginUserViewModel login);
        Task<User> GetUserByPhoneNumbers(string phoneNumber);
        Task<ActiveAccountResult> ActiveAcount(ActiveAccountViewModel activeAccount);
        Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword);
        Task<ActiveForgotPasswordResult> ActiveForgotPassword(ActiveForgotPasswordViewModel activeForgot);
        Task<ResetPasswordResult> ResetPassword(ResetPasswordViewModel resetPassword);
        Task<User> GetUserById(long userid);
      
        #endregion

        #region profile
        Task<InformationUserProfileViewModel> GetInformationUserProfile(string phoneNumber); 
        Task<EditUserProfileViewModel> GetEditUserProfile(long userId);
        Task<EditUserProfileResult> EditUserProfile(long userId, EditUserProfileViewModel editUserProfile);
        Task<ChangePasswordResult> ChangePassword(long userId, ChangePasswordViewModel changePassword);
        Task<Address> GetAddresByUserId(long userId);

        Task<CreateAddressResult> CreateAddress(long userId, CreateAddressViewModel create);
        Task<EditAddressResult> EditAddress(EditAddressViewModel edit);
        Task<CreateAddressViewModel> GetCreateAddress(long userId);
        Task<EditAddressViewModel> GetEditAddress(long userId,long addressId);
        Task<CreateOrEditAddressResult> CreateOrEditAddress(long userId, CreateOrEditAddressViewModel createOrEdit);
      
        #endregion

        #region Admin
        Task<FilterUserViewModel> FilterUser(FilterUserViewModel filter);

        Task<EditUserForAdminViewModel> GetEditUserForAdmin(long userId);
        Task<EditUserForAdminResult> EditUserForAdmin(EditUserForAdminViewModel editUser);
        Task<long> CreateUserForAdmin(CreateUserForAdminViewModel createUser);
        Task AddRoleToUser(List<long> RoleIds, long userId);
        #endregion


    }
}
