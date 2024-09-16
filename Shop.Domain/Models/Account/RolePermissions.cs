using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Account
{
  public  class RolePermissions: BaseEntitity
    {
        public long RoleId { get; set; }
        public long PermissionId  { get; set; }
       


        #region Navigation 
        public Permission Permission { get; set; }
        public Role Role { get; set; } 
        #endregion
    }
}
