using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ISmsServece _smsServece;
        private readonly IWalletService _walletService;
        public UserServices(IUserRepository userRepository, IPasswordHelper passwordHelper, ISmsServece smsServece, IWalletService walletService)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _smsServece = smsServece;
            _walletService = walletService;
        }


        #region Account
        public async Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register)
        {
            var user = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                PhoneNumber = register.PhoneNumber,
                Password = _passwordHelper.EncodePasswordMd5(register.Password),
                IsActive = false,
                ActiveCode = new Random().Next(10000, 99999).ToString(),
                Isblocked = false,
                IsDelete = false

            };
            await _userRepository.AddUser(user);
            await _userRepository.SaveChange();
            await _smsServece.SendVerifictionCode(user.PhoneNumber, user.ActiveCode);
            return RegisterUserResult.Success;
        }
        public async Task AgainActiveCode(RegisterUserViewModel register)
        {
            var user =await _userRepository.GetUserByPhoneNumber(register.PhoneNumber);
            user.ActiveCode = new Random().Next(10000, 99999).ToString();
            _userRepository.UpdateUser(user);

            if (user.IsActive == false)
            {
                await _smsServece.SendVerifictionCode(user.PhoneNumber, user.ActiveCode);
            }
        }
        public async Task<bool> CheckActiveCode(RegisterUserViewModel register)
        {
            var user = await _userRepository.GetUserByPhoneNumber(register.PhoneNumber);
            if (user.IsActive == false)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginUserResult> LoginUser(LoginUserViewModel login)
        {
            var user = await _userRepository.GetUserByPhoneNumber(login.PhoneNumber);
            if (user == null) return LoginUserResult.NotFound;
            if (user.Isblocked) return LoginUserResult.IsBlock;
            if (!user.IsActive) return LoginUserResult.NoActive;
            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password)) return LoginUserResult.NotFound;

            return LoginUserResult.success;

        }

        public async Task<User> GetUserByPhoneNumbers(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public async Task<ActiveAccountResult> ActiveAcount(ActiveAccountViewModel activeAccount)
        {
            var user = await _userRepository.GetUserByPhoneNumber(activeAccount.PhoneNumber);
            if (user == null) return ActiveAccountResult.NotFound;
            if (user.ActiveCode == activeAccount.ActiveCode)
            {
                user.IsActive = true;
                user.ActiveCode = new Random().Next(10000, 99999).ToString();
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChange();
                return ActiveAccountResult.Success;
            }
            return ActiveAccountResult.Error;
        }

        public async Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var user = await _userRepository.GetUserByPhoneNumber(forgotPassword.PhoneNumber);
            if (user == null)
            {
                return ForgotPasswordResult.NotFound;
            }
               await _smsServece.SendVerifictionCode(user.PhoneNumber, user.ActiveCode);

            return ForgotPasswordResult.Success;

        }

        public async Task<ActiveForgotPasswordResult> ActiveForgotPassword(ActiveForgotPasswordViewModel activeForgot)
        {
            var user = await _userRepository.GetUserByPhoneNumber(activeForgot.PhoneNumber);
            if (user == null) return ActiveForgotPasswordResult.NotFound;
            if (user.ActiveCode == activeForgot.ActiveCode)
            {

                return ActiveForgotPasswordResult.Success;
            }
            else
            {
                user.ActiveCode = new Random().Next(10000, 99999).ToString();
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChange();
                return ActiveForgotPasswordResult.Error;
            }

        }

        public async Task<ResetPasswordResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var user = await _userRepository.GetUserByActiveCode(resetPassword.ActiveCode);
            if (user == null) return ResetPasswordResult.NotFound;
            user.ActiveCode = new Random().Next(10000, 99999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(resetPassword.Password);
            _userRepository.UpdateUser(user);
            await _userRepository.SaveChange();
            return ResetPasswordResult.Success;

        }

        public async Task<User> GetUserById(long userid)
        {
            return await _userRepository.GetUserById(userid);
        }
    


        #endregion

        #region profile
        public async Task<InformationUserProfileViewModel> GetInformationUserProfile(string phoneNumber)
        {
            var user = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            var info = new InformationUserProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreateDate = user.CreateDate,
                PhoneNumber = user.PhoneNumber,
                WalletAmount = await _walletService.BalanceWallet(phoneNumber)

            };
            return info;
        }

        public async Task<EditUserProfileViewModel> GetEditUserProfile(long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;

            return new EditUserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber

            };

        }
        public async Task<EditUserProfileResult> EditUserProfile(long userId, EditUserProfileViewModel editUserProfile)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return EditUserProfileResult.NotFound;

            user.FirstName = editUserProfile.FirstName;
            user.LastName = editUserProfile.LastName;

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChange();

            return EditUserProfileResult.Success;
        }

        public async Task<ChangePasswordResult> ChangePassword(long userId, ChangePasswordViewModel changePassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return ChangePasswordResult.NotFound;
            var oldPass = _passwordHelper.EncodePasswordMd5(changePassword.OldPassword);
            if (user.Password == oldPass)
            {
                user.Password = _passwordHelper.EncodePasswordMd5(changePassword.Password);
                _userRepository.UpdateUser(user);
                await _userRepository.SaveChange();
                return ChangePasswordResult.Success;
            }
            return ChangePasswordResult.Erorr;
        }


        public async Task<Address> GetAddresByUserId(long userId)
        {
            return await _userRepository.GetAddresByUserId(userId);
        }

        public async Task<CreateAddressResult> CreateAddress(long userId, CreateAddressViewModel create)
        {
            var addressId = await _userRepository.GetAddresByUserId(userId);
            if (addressId != null) return CreateAddressResult.NotFound;
            var address = new Address()
            {
                UserId = userId,
                State = create.State,
                City = create.City,
                PostalCode = create.PostalCode,
                FullAddress = create.FullAddress
            };

            await _userRepository.AddAddress(address);
            await _userRepository.SaveChange();
            return CreateAddressResult.Success;


        }
        public async Task<EditAddressResult> EditAddress(EditAddressViewModel edit)
        {
            var address = await _userRepository.GetAddressById(edit.Id);
            if (address == null) return EditAddressResult.NotFound;
            address.State = edit.State;
            address.City = edit.City;
            address.PostalCode = edit.PostalCode;
            address.FullAddress = edit.FullAddress;

            _userRepository.UpdateAddress(address);
            await _userRepository.SaveChange();
            return EditAddressResult.Success;
        }

        public async Task<CreateAddressViewModel> GetCreateAddress(long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return new CreateAddressViewModel
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
        }
        public async Task<EditAddressViewModel> GetEditAddress(long userId, long addressId)
        {
            var user = await _userRepository.GetUserById(userId);
            var address = await _userRepository.GetAddressById(addressId);
            return new EditAddressViewModel
            {
                Id = address.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserId = address.UserId,
                State = address.State,
                City = address.City,
                PostalCode = address.PostalCode,
                FullAddress = address.FullAddress
            };
        }

        public async Task<CreateOrEditAddressResult> CreateOrEditAddress(long userId, CreateOrEditAddressViewModel createOrEdit)
        {
            var addressId = await _userRepository.GetAddresByUserId(userId);
            
            if (addressId != null)
            {
                //Edit
                var address = await _userRepository.GetAddressById(createOrEdit.Id);
                if (address == null) return CreateOrEditAddressResult.NotFound;
                address.State = createOrEdit.State;
                address.City = createOrEdit.City;
                address.PostalCode = createOrEdit.PostalCode;
                address.FullAddress = createOrEdit.FullAddress;

                _userRepository.UpdateAddress(address);
                await _userRepository.SaveChange();
                return CreateOrEditAddressResult.Success;
            }
            else
            {
                //Create
                var address = new Address()
                {
                    UserId = userId,
                    State = createOrEdit.State,
                    City = createOrEdit.City,
                    PostalCode = createOrEdit.PostalCode,
                    FullAddress = createOrEdit.FullAddress
                };

                await _userRepository.AddAddress(address);
                await _userRepository.SaveChange();
                return CreateOrEditAddressResult.Success;
            }
        }


        #endregion

        #region Admin
        public async Task<FilterUserViewModel> FilterUser(FilterUserViewModel filter)
        {
            return await _userRepository.FilterUser(filter);
        }
        public async Task<long> CreateUserForAdmin(CreateUserForAdminViewModel createUser)
        {
            var user = new User
            {
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                PhoneNumber = createUser.PhoneNumber,
                Password = _passwordHelper.EncodePasswordMd5(createUser.Password),
                IsActive = true,
                ActiveCode = new Random().Next(10000, 99999).ToString(),
                Isblocked = false,
                IsDelete = false
            };


            return await _userRepository.AddUserForAdmin(user);

        }

        public async Task<EditUserForAdminViewModel> GetEditUserForAdmin(long userId)
        {
            return await _userRepository.GetEditUserForAdmin(userId);
        }

        public async Task<EditUserForAdminResult> EditUserForAdmin(EditUserForAdminViewModel editUser)
        {
            var user = await _userRepository.GetUserById(editUser.Id);
            if (user == null) return EditUserForAdminResult.NotFound;
            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = _passwordHelper.EncodePasswordMd5(editUser.Password);
            }
            _userRepository.UpdateUser(user);
            await _userRepository.RemoveAllRoleSelected(editUser.Id);

            await _userRepository.AddRoleToUser(editUser.SelectedRoles, editUser.Id);
            await _userRepository.SaveChange();
            return EditUserForAdminResult.Success;
        }

        public async Task AddRoleToUser(List<long> RoleIds, long userId)
        {
            await _userRepository.AddRoleToUser(RoleIds, userId);
            await _userRepository.SaveChange();
        }

       

        #endregion
    }
}
