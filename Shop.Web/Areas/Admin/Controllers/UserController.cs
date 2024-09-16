using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Account;
using Shop.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IUserServices _userServices;
        private readonly IPermissionServices _permissionServices;
        public UserController(IUserServices userServices, IPermissionServices permissionServices)
        {
            _userServices = userServices;
            _permissionServices = permissionServices;
        }
        #region Get All Show User
       [PermissionCheker(2)]
        public async Task<IActionResult> Index(FilterUserViewModel filter)
        {

            return View(await _userServices.FilterUser(filter));
        }

        #endregion
        #region Edit User

        [PermissionCheker(4)]
        [HttpGet]
        public async Task<IActionResult> Edit(long userId)
        {
            ViewData["Roles"] = await _permissionServices.GetAllRole();
            var user = await _userServices.GetEditUserForAdmin(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
       
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserForAdminViewModel editUser)
        {
            ViewData["Roles"] = await _permissionServices.GetAllRole();
            if (ModelState.IsValid)
            {
               
                    var result = await _userServices.EditUserForAdmin(editUser);

                    switch (result)
                    {
                 
                    case EditUserForAdminResult.Success:
                            TempData[SuccessMessage] = "ویرایش کاربر با موفقیت انجام شد";
                            return RedirectToAction("Index");
                        case EditUserForAdminResult.NotFound:
                            TempData[WarningMessage] = "کاربری یافت نشد";
                            break;

                    }
                
            }
            return View();
        }

        #endregion

        #region Create User
        [PermissionCheker(3)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Roles"] = await _permissionServices.GetAllRole();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserForAdminViewModel create)
        {
            ViewData["Roles"] = await _permissionServices.GetAllRole();
            if (ModelState.IsValid)
            {
                try
                {
                    if (create.SelectedRoles == null)
                    {
                        TempData[WarningMessage] = "لطفا یک نقش را انتخاب کنید";
                    }
                    else
                    {
                        var userid = await _userServices.CreateUserForAdmin(create);
                        await _userServices.AddRoleToUser(create.SelectedRoles, userid);
                        TempData[SuccessMessage] = "افزودن کاربر جدید با موفقیت انجام شد";
                        return RedirectToAction("index");
                    }



                }
                catch (Exception ex)
                {
                    Exception innerException = ex;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }

                    if (innerException.Message.Contains("Violation of UNIQUE KEY constraint"))
                    {
                        ModelState.AddModelError("PhoneNumber", "این شماره موبایل قبلا ثبت شده است");

                    }
                }

            }
            return View(create);

        }
        #endregion
    }
}
