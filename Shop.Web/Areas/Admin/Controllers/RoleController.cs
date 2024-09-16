using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Permission;
using Shop.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class RoleController : BaseAdminController
    {
        private readonly IPermissionServices _permissionServices;
        public RoleController(IPermissionServices permissionServices)
        {
            _permissionServices = permissionServices;
        }
        [PermissionCheker(5)]
        public async Task<IActionResult> Index()
        {
            return View(await _permissionServices.GetAllRole());
        }
        [PermissionCheker(6)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Permissions"] = await _permissionServices.GetAllPermissions();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel create)
        {
            ViewData["Permissions"] = await _permissionServices.GetAllPermissions();
            if (ModelState.IsValid)
            {
                if (create.SelectedPermission == null)
                {
                    TempData[ErrorMessage] = "یک نقش را انتخاب کنید";
                }
                else
                {
                    var roleId = await _permissionServices.CreateRole(create);
                    await _permissionServices.AddPermissionToRole(create.SelectedPermission, roleId);
                    TempData[SuccessMessage] = "افزودن نقش جدید با موفقیت انجام شد";
                    return RedirectToAction("Index");
                }
               

            }
            return View(create);
        }
        [PermissionCheker(7)]
        [HttpGet]
        public async Task<IActionResult> Edit(long roleId)
        {
            ViewData["Permissions"] = await _permissionServices.GetAllPermissions();
            return View(await _permissionServices.GetEditRole(roleId));
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel edit)
        {
            ViewData["Permissions"] = await _permissionServices.GetAllPermissions();
            if (ModelState.IsValid)
            {
                var result = await _permissionServices.EditRole(edit);
                switch (result)
                {
                    case EditRoleResult.NotFound:
                        TempData[WarningMessage] = "نقشی یافت نشد";
                        break;
                    case EditRoleResult.Success:
                        TempData[SuccessMessage] = "ویرایش نقش جدید با موفقیت انجام شد";
                        return RedirectToAction("Index");

                }


            }
            return View(edit);
        }
    }
}
