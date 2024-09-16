using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Admin.Permission;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositores
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ShopDbContext _context;
        public PermissionRepository(ShopDbContext context)
        {
            _context = context;
        }
        #region Role
        public Task<List<Role>> GetAllRole()
        {
            return _context.Roles.ToListAsync();
        }
        public async Task<long> AddRole(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<EditRoleViewModel> GetEditRole(long roleId)
        {
            return await _context.Roles.AsQueryable()
                .Include(r => r.RolePermissions)
                  .Where(r => r.Id == roleId).Select(x => new EditRoleViewModel
                  {
                      RoleId = x.Id,
                      RoleTitle = x.RoleTitle,
                      SelectedPermission = x.RolePermissions.Select(r => r.PermissionId).ToList()

                  }).SingleOrDefaultAsync();
        }

        public async Task<Role> GetRoleById(long roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        #endregion


        #region Permission
        public async Task<List<Permission>> GetAllPermissions()
        {
            return await _context.Permissions.Where(p => !p.IsDelete).ToListAsync();
        }

        public async Task RemoveAllPermissionSelectedRole(long roleId)
        {
            var allRolePrmisions = await _context.RolePermissions.Where(r => r.RoleId == roleId).ToListAsync();
            if (allRolePrmisions.Any())
            {
                _context.RolePermissions.RemoveRange(allRolePrmisions);
            }

        }

        public async Task AddPermissionToRole(List<long> SelectedPermission, long roleId)
        {
            if (SelectedPermission != null && SelectedPermission.Any())
            {
                var rolePermissins = new List<RolePermissions>();
                foreach (var permissinId in SelectedPermission)
                {
                    rolePermissins.Add(new RolePermissions()
                    {
                        PermissionId = permissinId,
                        RoleId = roleId
                    });
                }
                await _context.RolePermissions.AddRangeAsync(rolePermissins);
            }
        }

        public bool ChekerPermission(long permissinId, string phoneNumber)
        {
            long userId = _context.Users.AsQueryable().Single(u => u.PhoneNumber == phoneNumber).Id;

            List<long> userRoles = _context.UserRoles.AsQueryable()
                    .Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();

            if (!userRoles.Any())
            {
                return false;
            }

            List<long> permissions = _context.RolePermissions.AsQueryable()
                .Where(p => p.PermissionId == permissinId).Select(p => p.RoleId).ToList();

            return permissions.Any(p => userRoles.Contains(p));

        }


        #endregion

    }
}
