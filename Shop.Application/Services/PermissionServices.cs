using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class PermissionServices : IPermissionServices
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionServices(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        #region Role
        public async Task<List<Role>> GetAllRole()
        {
            return await _permissionRepository.GetAllRole();
        }
        public async Task<long> CreateRole(CreateRoleViewModel createRole)
        {
            var role = new Role
            {
                RoleTitle = createRole.RoleTitle
            };

            return await _permissionRepository.AddRole(role);

        }

        public async Task<EditRoleViewModel> GetEditRole(long roleId)
        {
            return await _permissionRepository.GetEditRole(roleId);
        }

        public async Task<EditRoleResult> EditRole(EditRoleViewModel editRole)
        {
            var role = await _permissionRepository.GetRoleById(editRole.RoleId);
            if (role == null) return EditRoleResult.NotFound;

            role.RoleTitle = editRole.RoleTitle;

            _permissionRepository.UpdateRole(role);


            await _permissionRepository.RemoveAllPermissionSelectedRole(editRole.RoleId);
            await _permissionRepository.AddPermissionToRole(editRole.SelectedPermission, editRole.RoleId);

            await _permissionRepository.SaveChange();
            return EditRoleResult.Success;
        }

        #endregion

        #region Permission
        public async Task<List<Permission>> GetAllPermissions()
        {
            return await _permissionRepository.GetAllPermissions();
        }

        public async Task AddPermissionToRole(List<long> SelectedPermission, long roleId)
        {
            await _permissionRepository.AddPermissionToRole(SelectedPermission, roleId);
            await _permissionRepository.SaveChange();
        }

        public bool ChekerPermission(long permissinId, string phoneNumber)
        {
            return _permissionRepository.ChekerPermission(permissinId, phoneNumber);
        }


        #endregion

    }
}
