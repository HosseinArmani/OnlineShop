using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        #region Role
        Task<List<Role>> GetAllRole();
        Task<long> AddRole(Role role);
        void UpdateRole(Role role);
        Task SaveChange();
        Task<EditRoleViewModel> GetEditRole(long roleId);
        Task<Role> GetRoleById(long roleId);
      
        #endregion


        #region Permission
        Task<List<Permission>> GetAllPermissions();
        Task RemoveAllPermissionSelectedRole(long roleId);
        Task AddPermissionToRole(List<long> SelectedPermission, long roleId);
        bool ChekerPermission(long permissinId,string phoneNumber);
    
        #endregion


    }
}
