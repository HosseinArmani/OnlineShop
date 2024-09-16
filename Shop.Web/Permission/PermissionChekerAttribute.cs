using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Permission
{
    public class PermissionChekerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IPermissionServices _permissionServices;
        private long _permissionId = 0;
        public PermissionChekerAttribute(long permissionId)
        {
            _permissionId = permissionId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permissionServices = (IPermissionServices)context.HttpContext.RequestServices.GetService(typeof(IPermissionServices));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var phoneNumber = context.HttpContext.User.Identity.Name;
                if (!_permissionServices.ChekerPermission(_permissionId, phoneNumber))
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
