using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
   public class InformationUserProfileViewModel
    {
  
        public string FirstName { get; set; }
    
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime CreateDate { get; set; }  
        public long WalletAmount { get; set; }


    }
}
