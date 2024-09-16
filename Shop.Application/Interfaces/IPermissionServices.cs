using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
   public interface IPermissionServices
    {
        #region Role
        Task<List<Role>> GetAllRole();
        Task<long> CreateRole(CreateRoleViewModel createRole);
        Task<EditRoleViewModel> GetEditRole(long roleId);
        Task<EditRoleResult> EditRole(EditRoleViewModel editRole);
       
        #endregion


        #region Permission
        Task<List<Permission>> GetAllPermissions();
        Task AddPermissionToRole(List<long> SelectedPermission, long roleId);
        bool ChekerPermission(long permissinId, string phoneNumber);
        #endregion

    }
}
