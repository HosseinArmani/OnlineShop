using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Account
{
    public class FilterUserViewModel : BasePaging
    {
        public string PhoneNumber { get; set; }
        public List<User> Users { get; set; }
        public FilterUserViewModel SetUser(List<User> users)
        {
            this.Users = users;
            return this;
        }
        #region method

        public FilterUserViewModel SetPaging(BasePaging basePaging)
        {
            this.PageId = basePaging.PageId;
            this.PageCount = basePaging.PageCount;
            this.AllEntityCount = basePaging.AllEntityCount;
            this.CountForShowAfterAndBefor = basePaging.CountForShowAfterAndBefor;
            this.EndPage = basePaging.EndPage;
            this.SkipEntitiy = basePaging.SkipEntitiy;
            this.StartPage = basePaging.StartPage;
            this.TakeEntity = basePaging.TakeEntity;

            return this;

        }

        #endregion
    }
}
