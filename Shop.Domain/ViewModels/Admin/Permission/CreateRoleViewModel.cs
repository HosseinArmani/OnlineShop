using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Permission
{
  public  class CreateRoleViewModel
    {
        public long RoleId { get; set; }
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string RoleTitle { get; set; }
        public List<long> SelectedPermission { get; set; }
    }
    public enum CreateRoleResult
    {
        Success,
        Notselected
    }
}
